using System;
using System.Linq;
using System.Collections.Generic;

using Quoridor.Core.Utils;
using ConcurrentCollections;
using Quoridor.Core.Entities;
using Quoridor.Common.Logging;
using Quoridor.Core.Environment;
using Quoridor.AI.AStarAlgorithm;
using Quoridor.Core.Utils.CustomExceptions;

namespace Quoridor.Core.Game
{
    public class GameEnvironment : IGameEnvironment
    {
        public EventHandler OnMoveDone { get; set; }

        private readonly AStar<Vector2, IBoard, IPlayer> _aStar = new();
        private readonly ILogger _log = Logger.InstanceFor<GameEnvironment>();
        private readonly object _lock = new();

        public int Turn { get; private set; }
        public IBoard Board {get; private set;}
        public List<IPlayer> Players { get; private set; }
        public ConcurrentHashSet<IWall> Walls { get; private set; }

        public GameEnvironment(
            int numPlayers,
            int numWalls,
            IBoard board)
        {
            Board = board;
            Players = new List<IPlayer>();
            Walls = new ConcurrentHashSet<IWall>();
            InitAndAddPlayers(numPlayers, numWalls);
        }

        public GameEnvironment() {

        }

        public void Initialize()
        {
            lock(_lock)
                Turn = 0;
            Board.Initialize();
            Walls.Clear();
            Players.ForEach(p => p.Initialize());
        }

        public void InitAndAddPlayers(int numPlayers, int numWalls)
        {
            Players.Clear();
            Initialize();

            var startXs = new int[4] { Board.Dimension / 2, Board.Dimension / 2, 0, Board.Dimension - 1 };
            var startYs = new int[4] { 0, Board.Dimension - 1, Board.Dimension / 2, Board.Dimension / 2 };
            var goalConditions = new IsGoal<Vector2>[] {
                (pos) => pos.Y == Board.Dimension - 1,
                (pos) => pos.Y == 0,
                (pos) => pos.X == Board.Dimension - 1,
                (pos) => pos.X == 0
            };
            var heuristics = new H_n<Vector2>[]
            {
                (pos) => Math.Abs(Board.Dimension - 1 - pos.Y),
                (pos) => pos.Y,
                (pos) => Math.Abs(Board.Dimension - 1 - pos.X),
                (pos) => pos.X
            };

            var currIdAscii = 65;

            for (int i = 0; i < numPlayers; i++)
            {
                var startX = startXs[i];
                var startY = startYs[i];
                var playerId = (char)currIdAscii++;

                var startPos = new Vector2(startX, startY);
                var player = new Player(playerId, numWalls, startPos)
                {
                    ManhattanHeuristicFn = heuristics[i],
                    IsGoalMove = goalConditions[i],
                };

                AddPlayer(player);
                _log.Info($"Successfully added player '{player}'. Start pos: '{startPos}'");
            }
        }

        public void AddPlayer(IPlayer player)
        {
            Players.Add(player);
            _log.Info($@"Added player '{player.Id}'. Available players: '{
                string.Join(", ", Players.Select(p => p.Id))}'");
        }

        public void ChangeTurn()
        {
            lock(_lock)
                Turn = (Turn + 1) % Players.Count;
        }

        public IPlayer CurrentPlayer
        {
            get
            {
                if (Players.Count() == 0)
                    throw new Exception($"No player registered");
                return Players[Turn];
            }
        }

        public IPlayer Opponent
        {
            get
            {
                if (Players.Count() == 0)
                    throw new Exception($"No player registered");
                return Players[PreviousTurn()];
            }
        }

        public bool HasFinished => Players.Any(p => p.IsGoalMove(p.CurrentPos));

        public IPlayer Winner => Players.FirstOrDefault(p => p.IsGoalMove(p.CurrentPos));

        public void MovePlayer(IPlayer player, Direction dir)
        {
            if (Players == null)
                throw new Exception(@$"player '{player}' not registered. Call the {
                    nameof(AddPlayer)} method to register");

            _log.Info($"Moving player '{player}' '{dir}'...");

            var possibleNewPositions = TryMove(player, new() { player.CurrentPos }, dir);
            var move = possibleNewPositions.First();

            if (possibleNewPositions.Count > 1)
            {
                var bestPath = double.MaxValue;
                Vector2 bestMove = null;
                foreach(var pos in possibleNewPositions)
                {
                    var temp = player.CurrentPos;
                    player.CurrentPos = pos;
                    var stats = _aStar.BestMove(Board, player);
                    if (stats.Value < bestPath)
                    {
                        bestPath = stats.Value;
                        bestMove = pos;
                    }
                    player.CurrentPos = temp;
                }
                move = bestMove;
            }

            player.Move(move);

            _log.Info($"Moved player '{player}' to '{move}'");
        }

        public void MovePlayer(IPlayer player, Vector2 newPos)
        {
            if (Players == null)
                throw new Exception(@$"player '{player}' not registered. Call the {nameof(AddPlayer)} method to register");

            _log.Info($"Moving player '{player}' '{newPos}'...");

            var possiblePlayerPositions = GetWalkableNeighbors(player).Cast<AgentMove>();
            // if wanted pos is in the list of available move pos, then move
            if (possiblePlayerPositions.Any(p => p.NewPos.Equals(newPos)))
            {
                player.Move(newPos);
            }
            else
            {
                throw new Exception($"Player at pos {player.CurrentPos} cannot move to {newPos}");
            }
            _log.Info($"Moved player '{player}' to '{newPos}'");
        }


        public void AddWall(IPlayer player, Vector2 from, Direction placement)
        {
            _log.Info($"Attempting to add '{placement}ern' wall from '{from}' for player '{player}'");

            //check if player has any wall remaining
            if (player.NumWalls <= 0)
                throw new NoWallRemainingException($"player {player} has no walls left");

            //check if wall is valid and within bounds
            var wall = CreateAndValidateWall(from, placement);

            //check if wall is already present
            if (Walls.Contains(wall))
                throw new WallAlreadyPresentException($"{wall} already present");

            //check if wall is already present or intersects with another wall
            var intersectingWall = Walls.FirstOrDefault(w => w.Intersects(from, placement));
            if (intersectingWall != default(IWall))
                throw new WallIntersectsException($"{wall} intersects with already present wall {intersectingWall}");

            //get 4 cells affected by the wall and block cell access
            var affectedCells = GetCellsAffectedByWall(wall);
            BlockAccess(affectedCells);

            //check if all players can move to their goal, and if not, unblock the path and throw
            if (ShouldCheckForBlockage())
            {
                var blockedPlayer = Players.FirstOrDefault(player => _aStar.BestMove(Board, player) is null);
                if (blockedPlayer != default(IPlayer))
                {
                    UnblockAccess(affectedCells);
                    throw new NewWallBlocksPlayerException(@$"{wall} blocks player {blockedPlayer}");
                }
            }
           
            //wall check complete, add it to the wall cache
            Walls.Add(wall);

            //player used up a wall, so decrease the wall count
            player.DecreaseWallCount();
        }

        private bool ShouldCheckForBlockage()
        {
            //case 1: there are 3 walls and player is at edge or 1 step away from edge
            //because player may have no possible path to goal
            return Walls.Count >= 2 && Players.Any(IsInCorners) || Walls.Count >= ((Board.Dimension / 2) - 1);
        }

        private bool IsInCorners(IPlayer player)
        {
            return
                player.CurrentPos.X <= 1 ||
                player.CurrentPos.X >= Board.Dimension - 3 ||
                player.CurrentPos.Y <= 1 ||
                player.CurrentPos.Y >= Board.Dimension - 3;
        }

        private void BlockAccess(IEnumerable<AffectedCell> affectedCells)
        {
            foreach (var affectedCell in affectedCells)
            {
                affectedCell.Cell.Block(affectedCell.BlockedDirection);
            }
        }

        private void UnblockAccess(IEnumerable<AffectedCell> affectedCells)
        {
            foreach (var affectedCell in affectedCells)
            {
                affectedCell.Cell.Unblock(affectedCell.BlockedDirection);
            }
        }

        public void RemoveWall(IPlayer player, Vector2 from, Direction placement)
        {
            _log.Info($"Attempting to remove '{placement}ern' wall from '{from}'");

            //check if wall is valid and within bounds
            var wall = CreateAndValidateWall(from, placement);

            //check if wall was previously added
            if (!Walls.Contains(wall))
                throw new WallNotPresentException($"{wall} not present'");

            //get cells affected by the wall and unblock access
            var affectedCells = GetCellsAffectedByWall(wall);

            UnblockAccess(affectedCells);

            //remove wall from the cache
            Walls.TryRemove(wall);

            player.IncreaseWallCount();

            _log.Info($"Successfully removed {wall}");
        }

        //each wall blocks one direction from exactly 4 cell
        public IEnumerable<AffectedCell> GetCellsAffectedByWall(IWall wall)
        {
            yield return new AffectedCell(Board.GetCell(wall.From), wall.Placement);

            var newPos = wall.From.Copy();
            if (wall.IsHorizontal()) newPos.X++;
            else newPos.Y++;

            yield return new AffectedCell(Board.GetCell(newPos), wall.Placement);

            var oppositeWall = wall.Opposite();

            yield return new AffectedCell(Board.GetCell(oppositeWall.From), oppositeWall.Placement);
            yield return new AffectedCell(Board.GetCellAt(newPos, wall.Placement), oppositeWall.Placement);
        }

        public IWall CreateAndValidateWall(Vector2 from, Direction dir)
        {
            if (!Board.WithinBounds(from))
            {
                var errorMessage = $"{dir}ern wall from '{from}' could not be removed. Invalid dimension";
                _log.Error(errorMessage);
                throw new InvalidWallException(errorMessage);
            }

            var wall = new Wall(dir, from);

            if ((wall.From.X == 0 && wall.Placement.Equals(Direction.West))
                || (wall.From.Y == 0 && wall.Placement.Equals(Direction.North))
                || (wall.From.X == Board.Dimension - 1 && wall.Placement.Equals(Direction.East))
                || (wall.From.Y == Board.Dimension - 1 && wall.Placement.Equals(Direction.South))
                || (wall.From.X >= Board.Dimension - 1 && wall.IsHorizontal())
                || (wall.From.Y >= Board.Dimension - 1 && wall.IsVertical()))
            {
                var errorMessage = $"{dir}ern wall from '{from} not possible.'";
                _log.Error(errorMessage);
                throw new InvalidWallException(errorMessage);
            }
            return wall;
        }

        private void CheckIfPlayerCanGoToNewPos(Vector2 currentPos, Vector2 newPos)
        {
            if (!Board.WithinBounds(newPos))
            {
                var errorMessage = $"player '{CurrentPlayer.Id}' cannot move to '{newPos}'. Invalid move position";
                _log.Error(errorMessage);
                throw new InvalidAgentMoveException(errorMessage);
            }

            //if newPos can't be reached from currentPos, then there's a wall blocking access between those cells
            if (!Board.GetCell(currentPos).IsAccessible(currentPos.GetDirFor(newPos)))
            {
                var errorMessage = $"player '{CurrentPlayer.Id}' cannot move to '{newPos}' since it's blocked by a wall";
                _log.Error(errorMessage);
                throw new NewMoveBlockedByWallException(errorMessage);
            }
        }

        private bool PlayerCanGoTo(Vector2 currentPos, Vector2 newPos)
        {
            try
            {
                CheckIfPlayerCanGoToNewPos(currentPos, newPos);
            }
            catch (Exception ex) when (ex is InvalidAgentMoveException || ex is NewMoveBlockedByWallException)
            {
                return false;
            }
            return true;
        }


        private List<Vector2> TryMove(IPlayer player, List<Vector2> currentPos, Direction dir, bool jump=false)
        {
            var newPos = currentPos.Select(c => c.GetPosFor(dir)).ToList();

            _log.Info($@"Trying to check if it's possible to move player '{player.Id}' currently at '{
                player.CurrentPos}' '{dir}' to '{newPos}' from '{currentPos}'");


            var newPosCopy = new List<Vector2>();
            for (int i = 0; i < newPos.Count(); i++)
            {
                try
                {
                    CheckIfPlayerCanGoToNewPos(currentPos[i], newPos[i]);
                    newPosCopy.Add(newPos[i]);
                }
                catch (Exception ex) when (ex is InvalidAgentMoveException || ex is NewMoveBlockedByWallException)
                {
                    if (jump)
                    {
                        newPosCopy.AddRange(GetSidewaysJumpPos(currentPos[i], dir));
                    }
                    else throw;
                }
            }
            newPos = newPosCopy;

            if (newPos.Any(n => player.IsGoalMove(n)))
            {
                _log.Info($"Player reached the goal position");
                return newPos;
            }

            var playerInNewPos = Players.FirstOrDefault(p => newPos.Contains(p.CurrentPos));

            if (playerInNewPos != null)
            {
                _log.Info($"Player '{playerInNewPos.Id}' already is in '{newPos}'. Trying to jump '{dir}' from '{newPos}'");
                return TryMove(player, newPos, dir, jump:true);
            }
            _log.Info($"No player found in '{newPos}'. Moving player '{player.Id}' from '{player.CurrentPos}' to '{newPos}'");
            return newPos;
        }

        private IEnumerable<Vector2> GetSidewaysJumpPos(Vector2 currentPos, Direction dir)
        {
            Vector2 leftSideWaypos;
            Vector2 rightSideWaypos;

            if (dir.Equals(Direction.North) || dir.Equals(Direction.South))
            {
                leftSideWaypos = currentPos.GetPosFor(Direction.West);
                rightSideWaypos = currentPos.GetPosFor(Direction.East);
            }
            else
            {
                leftSideWaypos = currentPos.GetPosFor(Direction.South);
                rightSideWaypos = currentPos.GetPosFor(Direction.North);
            }
            var leftPossible = PlayerCanGoTo(currentPos, leftSideWaypos);
            var rightPossible = PlayerCanGoTo(currentPos, rightSideWaypos);

            if (!leftPossible && !rightPossible)
                throw new PlayerCannotJumpSidewaysException($"{CurrentPlayer} cannot jump sideways, hence cannot move to {dir}ern direction");

            if (leftPossible)
                yield return leftSideWaypos;

            if (rightPossible)
                yield return rightSideWaypos;
        }

        public void Move(Movement move)
        {
            ValidateNotNull(move, nameof(move));

            if (move is AgentMove agentMove)
            {
                agentMove.CurrentPos = CurrentPlayer.CurrentPos;
                if (agentMove.NewPos is not null)
                    MovePlayer(CurrentPlayer, agentMove.NewPos);
                else
                    MovePlayer(CurrentPlayer, agentMove.Dir);
            }

            else if (move is Wall wallMove)
                AddWall(CurrentPlayer, wallMove.From, wallMove.Placement);

            else if (move is Vector2 vecMove)
            {
                var dir = CurrentPlayer.CurrentPos.GetDirFor(vecMove);
                MovePlayer(CurrentPlayer, dir);
            }
            else throw new Exception($"move type {move.GetType().Name} not supported");

            ChangeTurn();
            OnMoveDone?.Invoke(this, null);
        }

        private int PreviousTurn()
        {
            return (Turn + Players.Count - 1) % Players.Count;
        }

        public void UndoMove(Movement move)
        {
            ValidateNotNull(move, nameof(move));

            //previous player index
            lock(_lock)
                Turn = PreviousTurn();

            if (move is AgentMove agentMove)
                CurrentPlayer.CurrentPos = agentMove.CurrentPos;

            else if (move is Wall wallMove)
                RemoveWall(CurrentPlayer, wallMove.From, wallMove.Placement);

            else throw new Exception($"move type {move.GetType().Name} not supported");
        }

        public IEnumerable<Movement> GetValidMoves()
        {
            return GetWalkableNeighbors(CurrentPlayer).Concat(GetAllUnplacedWalls());
        }

        public IEnumerable<Movement> GetWalkableNeighbors(IPlayer player)
        {
            //if player already at goal, no need to return neighbors
            if (player.IsGoal(player.CurrentPos))
                yield break;

            //we do this to ensure any possible jumps won't be blocked by a wall
            foreach(var validDir in Board.NeighborDirs(player.CurrentPos))
            {
                List<Vector2> moves;
                try
                {
                    moves = TryMove(player, new() { player.CurrentPos }, validDir);
                }
                catch(MoveException) {
                    continue;
                }
                foreach (var move in moves)
                    yield return new AgentMove(validDir, move);
            }
        }

        public IEnumerable<Movement> GetAllUnplacedWalls()
        {
            //all wall pieces that can be placed on the board. We need to check if the wall
            //isn't already present or intersects with another wall, and also the wall should
            //not block any player from reaching the goal.

            //if player has no wall remaining, we don't need to add in wall moves.
            if (CurrentPlayer.NumWalls <= 0)
                return Enumerable.Empty<Movement>();

            return
            //horizontal walls
            GetValidWallsInDir(0, Board.Dimension - 2, 1, Board.Dimension - 1, Direction.North)
            //vertical walls
            .Concat(GetValidWallsInDir(1, Board.Dimension - 1, 0, Board.Dimension - 2, Direction.West));
        }

        private IEnumerable<Movement> GetValidWallsInDir(int x0, int x1, int y0, int y1, Direction dir)
        {
            for (int i = x0; i <= x1; i++)
            {
                for (int j = y0; j <= y1; j++)
                {
                    var from = new Vector2(i, j);
                    // check if wall overlaps/intersects another wall, if so, ignore
                    if (Walls.Any(wall => wall.Intersects(from, dir)))
                        continue;

                    //check if adding in this wall blocks any players
                    try
                    {
                        AddWall(CurrentPlayer, from, dir);
                        RemoveWall(CurrentPlayer, from, dir);
                    }
                    catch (WallException)
                    {
                        continue;
                    }

                    yield return new Wall(dir, from);
                }
            }
        }

        public double Evaluate(bool currentMaximizer)
        {
            var result = _aStar.BestMove(Board, CurrentPlayer);
            var goalDistance = result.Value;
            var wallsLeft = CurrentPlayer.NumWalls;

            var result2 = _aStar.BestMove(Board, Opponent);
            var goalDistance2 = result2.Value;
            var wallsLeft2 = Opponent.NumWalls;

            double score;

            if (!currentMaximizer) // previous is maximizer (>= 0 means strong pos for prev)
            {
                score = goalDistance - goalDistance2 + (wallsLeft - wallsLeft2);

                if (CurrentPlayer.NumWalls == 0) score -= Opponent.NumWalls * 2;

                if (CurrentPlayer.IsGoalMove(CurrentPlayer.CurrentPos))
                {
                    score = -100;
                }
                if (Opponent.IsGoalMove(Opponent.CurrentPos))
                {
                    score = 100;
                }
            }
            else // current is maximizer (>= 0 means strong pos for curr)
            {
                score = goalDistance2 - goalDistance + (wallsLeft2 - wallsLeft);

                if (Opponent.NumWalls == 0) score += CurrentPlayer.NumWalls * 2;

                if (CurrentPlayer.IsGoalMove(CurrentPlayer.CurrentPos))
                {
                    score = 100;
                }
                if (Opponent.IsGoalMove(Opponent.CurrentPos))
                {
                    score = -100;
                }
            }

            return score;
        }

        public IEnumerable<Vector2> Neighbors(Vector2 pos)
        {
            return Board.Neighbors(pos);
        }

        public IEnumerable<Movement> Neighbors(Movement pos)
        {
            var posVec = pos as Vector2;
            return Neighbors(posVec);
        }

        public IGameEnvironment DeepCopy()
        {
            var boardCopy = Board.DeepCopy();
            var playerCopy = new List<IPlayer>(Players.Select(p => p.DeepCopy()));
            var wallCopy = new ConcurrentHashSet<IWall>(Walls.Select(wall => wall.DeepCopy()));
            return new GameEnvironment
            {
                Board = boardCopy,
                Players = playerCopy,
                Walls = wallCopy,
                Turn = Turn
            };
        }

        private void ValidateNotNull<T>(T param, string paramName)
        {
            if (param is null)
                throw new ArgumentNullException(paramName);
        }

        public IEnumerable<Movement> RandomizableMoves()
        {
            return GetAllUnplacedWalls();
        }
    }
}
