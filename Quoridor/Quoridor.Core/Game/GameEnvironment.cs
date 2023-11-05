using System;
using System.Linq;
using System.Collections.Generic;

using Quoridor.Core.Utils;
using Quoridor.Core.Entities;
using Quoridor.Common.Logging;
using Quoridor.Core.Extensions;
using Quoridor.Core.Environment;
using Quoridor.AI.AStarAlgorithm;
using Quoridor.Core.Utils.CustomExceptions;
using Quoridor.AI.Interfaces;

namespace Quoridor.Core.Game
{
    public class GameEnvironment : IGameEnvironment
    {
        private readonly IBoard _board;

        public int Turn { get; private set; }
        public List<IPlayer> Players { get; private set; }
        public HashSet<IWall> Walls { get; private set; }
        private HashSet<IWall> AvailableWalls { get; set; }

        private readonly AStar<Vector2, IBoard, IPlayer> _aStar;

        private readonly ILogger _log = Logger.InstanceFor<GameEnvironment>();

        public GameEnvironment(
            int numPlayers,
            int numWalls,
            IBoard board)
        {
            _board = board;
            _aStar = new AStar<Vector2, IBoard, IPlayer>();
            Players = new List<IPlayer>();
            Walls = new HashSet<IWall>();
            InitAndAddPlayers(numPlayers, numWalls);
        }

        public void Initialize()
        {
            Turn = 0;
            _board.Initialize();
            Walls.Clear();
            Players.ForEach(p => p.Initialize());
        }

        private void InitAndAddPlayers(int numPlayers, int numWalls)
        {
            var startXs = new int[4] { _board.Dimension / 2, _board.Dimension / 2, 0, _board.Dimension - 1 };
            var startYs = new int[4] { 0, _board.Dimension - 1, _board.Dimension / 2, _board.Dimension / 2 };
            var goalConditions = new IsGoal<Vector2>[] {
                (pos) => pos.Y == _board.Dimension - 1,
                (pos) => pos.Y == 0,
                (pos) => pos.X == 0,
                (pos) => pos.X == _board.Dimension - 1
            };
            var heuristics = new H_n<Vector2>[]
            {
                (pos) => Math.Abs(_board.Dimension - 1 - pos.Y),
                (pos) => pos.Y,
                (pos) => pos.X,
                (pos) => Math.Abs(_board.Dimension - 1 - pos.Y)
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
                    IsGoalMove = goalConditions[i]
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

        public IPlayer PreviousPlayer
        {
            get
            {
                if (Players.Count() == 0)
                    throw new Exception($"No player registered");
                return Players[PreviousTurn()];
            }
        }

        public bool HasFinished => Players.Any(p => p.IsGoalMove(p.CurrentPos));

        public void MovePlayer(IPlayer player, Direction dir)
        {
            if (Players == null)
                throw new Exception(@$"player '{player}' not registered. Call the {
                    nameof(AddPlayer)} method to register");

            _log.Info($"Moving player '{player}' '{dir}'...");

            var newPos = TryMove(player, player.CurrentPos, dir);
            player.CurrentPos = newPos;

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
            var blockedPlayer = Players.FirstOrDefault(player => _aStar.BestMove(_board, player) is null);
            if (blockedPlayer != default(IPlayer))
            {
                UnblockAccess(affectedCells);
                throw new NewWallBlocksPlayerException(@$"{wall} blocks player");
            }
            //wall check complete, add it to the wall cache
            Walls.Add(wall);
            //player used up a wall, so decrease the wall count
            player.DecreaseWallCount();
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
            Walls.Remove(wall);

            _log.Info($"Successfully removed {wall}");

            player.IncreaseWallCount();
        }

        //each wall blocks one direction from exactly 4 cell
        public IEnumerable<AffectedCell> GetCellsAffectedByWall(IWall wall)
        {
            yield return new AffectedCell(_board.GetCell(wall.From), wall.Placement);

            var newPos = wall.From.Copy();
            if (wall.IsHorizontal()) newPos.X++;
            else newPos.Y++;

            yield return new AffectedCell(_board.GetCell(newPos), wall.Placement);

            var oppositeWall = wall.Opposite();

            yield return new AffectedCell(_board.GetCell(oppositeWall.From), oppositeWall.Placement);
            yield return new AffectedCell(_board.GetCellAt(newPos, wall.Placement), oppositeWall.Placement);
        }

        public IWall CreateAndValidateWall(Vector2 from, Direction dir)
        {
            if (!_board.WithinBounds(from))
            {
                var errorMessage = $"{dir}ern wall from '{from}' could not be removed. Invalid dimension";
                _log.Error(errorMessage);
                throw new InvalidWallException(errorMessage);
            }

            var wall = new Wall(dir, from);

            if ((wall.From.X == 0 && wall.Placement.Equals(Direction.West))
                || (wall.From.Y == 0 && wall.Placement.Equals(Direction.North))
                || (wall.From.X == _board.Dimension - 1 && wall.Placement.Equals(Direction.East))
                || (wall.From.Y == _board.Dimension - 1 && wall.Placement.Equals(Direction.South))
                || (wall.From.X >= _board.Dimension - 1 && wall.IsHorizontal())
                || (wall.From.Y >= _board.Dimension - 1 && wall.IsVertical()))
            {
                var errorMessage = $"{dir}ern wall from '{from} not possible.'";
                _log.Error(errorMessage);
                throw new InvalidWallException(errorMessage);
            }
            return wall;
        }

        private Vector2 TryMove(IPlayer player, Vector2 currentPos, Direction dir)
        {
            var newPos = currentPos.GetPosFor(dir);

            _log.Info($@"Trying to check if it's possible to move player '{player.Id}' currently at '{
                player.CurrentPos}' '{dir}' to '{newPos}' from '{currentPos}'");

            if (!_board.WithinBounds(newPos))
            {
                var errorMessage = $"player '{player.Id}' cannot move to '{newPos}'. Invalid move position";
                _log.Error(errorMessage);
                throw new InvalidAgentMoveException(errorMessage);
            }

            //if newPos can't be reached from currentPos, then there's a wall blocking access between those cells
            if (!_board.GetCell(currentPos).IsAccessible(currentPos.GetDirFor(newPos)))
            {
                var errorMessage = $"player '{player.Id}' cannot move to '{newPos}' since it's blocked by a wall";
                _log.Error(errorMessage);
                throw new NewMoveBlockedByWallException(errorMessage);
            }

            var playerInNewPos = Players.FirstOrDefault(p => p.CurrentPos.Equals(newPos));

            if (playerInNewPos != null)
            {
                _log.Info($"Player '{playerInNewPos.Id}' already is in '{newPos}'. Trying to jump '{dir}' from '{newPos}'");
                return TryMove(player, newPos, dir);
            }
            _log.Info($"No player found in '{newPos}'. Moving player '{player.Id}' from '{player.CurrentPos}' to '{newPos}'");
            return newPos;
        }

        public void Move(Movement move)
        {
            ValidateNotNull(move, nameof(move));

            if (move is AgentMove agentMove)
                MovePlayer(CurrentPlayer, agentMove.Dir);

            else if (move is Wall wallMove)
                AddWall(CurrentPlayer, wallMove.From, wallMove.Placement);

            else if (move is Vector2 vecMove)
            {
                var dir = CurrentPlayer.CurrentPos.GetDirFor(vecMove);
                MovePlayer(CurrentPlayer, dir);
            }
            else throw new Exception($"move type {move.GetType().Name} not supported");

            ChangeTurn();
        }

        private int PreviousTurn()
        {
            return (Turn + Players.Count - 1) % Players.Count;
        }

        public void UndoMove(Movement move)
        {
            ValidateNotNull(move, nameof(move));

            //previous player index
            Turn = PreviousTurn();

            if (move is AgentMove agentMove)
                MovePlayer(CurrentPlayer, agentMove.Dir.Opposite());

            else if (move is Wall wallMove)
                RemoveWall(CurrentPlayer, wallMove.From, wallMove.Placement);

            else throw new Exception($"move type {move.GetType().Name} not supported");
        }

        public IEnumerable<Movement> GetValidMoves()
        {
            return GetWalkableNeighbors().Concat(GetAllUnplacedWalls());
        }

        public IEnumerable<Movement> GetWalkableNeighbors()
        {
            //if player already at goal, no need to return neighbors
            if (CurrentPlayer.IsGoal(CurrentPlayer.CurrentPos))
                yield break;

            //we do this to ensure any possible jumps won't be blocked by a wall
            foreach(var validDir in _board.NeighborDirs(CurrentPlayer.CurrentPos))
            {
                try
                {
                    TryMove(CurrentPlayer, CurrentPlayer.CurrentPos, validDir);
                }
                catch(Exception) {
                    continue;
                }
                yield return new AgentMove(validDir);
            }
        }

        private IEnumerable<Movement> GetAllUnplacedWalls()
        {
            //all wall pieces that can be placed on the board. We need to check if the wall
            //isn't already present or intersects with another wall, and also the wall should
            //not block any player from reaching the goal.

            //if player has no wall remaining, we don't need to add in wall moves.
            if (CurrentPlayer.NumWalls <= 0)
                return Enumerable.Empty<Movement>();

            return
            //horizontal walls
            GetValidWallsInDir(0, _board.Dimension - 3, 1, _board.Dimension - 2, Direction.North)
            //vertical walls
            .Concat(GetValidWallsInDir(1, _board.Dimension - 2, 0, _board.Dimension - 3, Direction.West));
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

        public double Evaluate()
        {
            var result = _aStar.BestMove(_board, CurrentPlayer);
            var goalDistance = result.Value;
            var wallsLeft = CurrentPlayer.NumWalls;

            var otherAgent = Players.Single(p => !p.Equals(CurrentPlayer));
            var result2 = _aStar.BestMove(_board, otherAgent);
            var goalDistance2 = result2.Value;
            var wallsLeft2 = otherAgent.NumWalls;

            var score = goalDistance - goalDistance2 + wallsLeft - wallsLeft2;
            return score;
        }

        public IEnumerable<Vector2> Neighbors(Vector2 pos)
        {
            return _board.Neighbors(pos);
        }

        public IEnumerable<Movement> Neighbors(Movement pos)
        {
            var posVec = pos as Vector2;
            return Neighbors(posVec);
        }

        private void ValidateNotNull<T>(T param, string paramName)
        {
            if (param is null)
                throw new ArgumentNullException(paramName);
        }
    }
}
