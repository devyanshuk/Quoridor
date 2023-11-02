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
using Quoridor.Core.Move;

namespace Quoridor.Core.Game
{
    public class GameEnvironment : IGameEnvironment
    {
        private readonly IBoard _board;

        public int Turn { get; private set; }
        public List<IPlayer> Players { get; private set; }
        public HashSet<IWall> Walls { get; private set; }

        int COUNT = 0;
        Wall WALL = new Wall(Direction.North, new Vector2(0, 1));

        private readonly AStar<Vector2, IBoard, IPlayer> _aStar;

        private readonly ILogger _log = Logger.InstanceFor<GameEnvironment>();

        public GameEnvironment(
            IBoard board)
        {
            _board = board;
            _aStar = new AStar<Vector2, IBoard, IPlayer>();
            Players = new List<IPlayer>();
            Walls = new HashSet<IWall>();
        }

        public void Initialize()
        {
            Turn = 0;
            _board.Initialize();
            Walls.Clear();
        }

        public void AddPlayer(IPlayer player)
        {
            Players.Add(player);
            _log.Info($@"Adding player '{player.Id}'. Available players: '{
                string.Join(", ", Players.Select(p => p.Id))}'");
        }

        public void ChangeTurn()
        {
            Turn = (Turn + 1) % Players.Count;
        }

        public IPlayer CurrentPlayer => Players?[Turn];

        public bool IsTerminal => Players?.Any(p => p.IsGoalMove(p.CurrentPos)) ?? false;

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

            if (player.NumWalls <= 0)
                throw new NoWallRemainingException($"player {player} has no walls left");

            if (!_board.WithinBounds(from))
            {
                var errorMessage = $"{placement}ern wall from '{from}' could not be added. Invalid dimension.";
                _log.Error(errorMessage);
                throw new InvalidWallException(errorMessage);
            }

            var walls = GetWallsForAffectedCells(from, placement);
            if (walls.All(w => _board.GetCell(w.From).IsAccessible(w.Placement)))
            {
                _log.Info(@$"Wall validity check complete. Adding wall '{from}': '{
                    placement}' to the board and checking if it's accessible for all players.");
                foreach (var wall in walls)
                    _board.GetCell(wall.From).AddWall(wall);
                CheckForBlockedPath(walls);
            }
            else
            {
                Console.WriteLine("Walls");
                foreach(var wall in Walls)
                    Console.WriteLine(wall);
                throw new WallAlreadyPresentException($"{placement}ern wall from '{from}' intersects with already present wall");
            }

            Walls.Add(walls.First());

            if (Walls.Contains(WALL))
            {
                COUNT++;
                Console.WriteLine($"WALL COUNT (ADD) = {COUNT}");
            }

            _log.Info($"Successfully added '{placement}ern' wall from '{from}'");

            player.DecreaseWallCount();
        }

        private void CheckForBlockedPath(IEnumerable<IWall> walls)
        {
            foreach(var player in Players)
            {
                var bestNextPath = _aStar.BestMove(_board, player);
                if (bestNextPath is null)
                {
                    //undo the walls first
                    foreach (var wall in walls)
                        _board.GetCell(wall.From).RemoveWall(wall);
                    throw new NewWallBlocksPlayerException($"{walls.First()} wall blocks player '{player}'");
                }
            }
        }

        public void RemoveWall(IPlayer player, Vector2 from, Direction placement)
        {
            _log.Info($"Attempting to remove '{placement}ern' wall from '{from}'");

            if (from.X == 0 && from.Y == 1) {
                Console.WriteLine($"Removing {placement} wall from {from}");
                Console.WriteLine("----------");
            }
            

            if (!_board.WithinBounds(from))
            {
                var errorMessage = $"{placement}ern wall from '{from}' could not be removed. Invalid dimension";
                _log.Error(errorMessage);
                throw new InvalidWallException(errorMessage);
            }

            var walls = GetWallsForAffectedCells(from, placement);
            if (walls.All(w => !_board.GetCell(w.From).IsAccessible(w.Placement)))
            {
                _log.Info($"'{placement}'ern wall from '{from}' exists. Removing it");
                foreach(var wall in walls)
                {
                    _board.GetCell(wall.From).RemoveWall(wall);
                }
            }
            else throw new WallNotPresentException($"{placement}ern wall from '{from} not present'");

            if (from.X == 0 && from.Y == 1)
            {
                Console.WriteLine($"Almost removing {placement}ern wall from {from}");
                foreach (var wall in Walls)
                    Console.WriteLine(wall);
            }

            if (Walls.Contains(WALL))
            {
                COUNT--;
                Console.WriteLine($"WALL COUNT (REMOVE)= {COUNT}");
            }
            Walls.Remove(walls.First());

            if (from.X == 0 && from.Y == 1)
            {
                Console.WriteLine($"REMOVED {placement}ern wall from {from}");
                foreach (var wall in Walls)
                    Console.WriteLine(wall);
            }

            _log.Info($"Successfully removed '{placement}ern' wall from '{from}'");

            if (Walls.Count > 0)
            {
                Console.WriteLine($"Removed {placement}ern wall from {from}");
                Console.WriteLine();
                foreach (var wall in Walls)
                    Console.WriteLine(wall);
                Console.WriteLine();
            }

            player.IncreaseWallCount();
        }

        public bool NewMoveBlockedByWall(Vector2 currPos, Vector2 newPos)
        {
            var neighbors = _board.Neighbors(currPos);
            return neighbors.All(n => !n.Equals(newPos));
        }

        public IEnumerable<IWall> GetWallsForAffectedCells(Vector2 from, Direction placement)
        {
            var wall = CreateAndValidateWall(from, placement);
            var wall2 = CreateAndValidateWall(_board.GetCellAt(from, placement).Position, placement.Opposite());

            var newPos = from.Copy();
            if (wall.IsHorizontal()) newPos.X++;
            else newPos.Y++;

            var wall3 = new Wall(placement, newPos);
            var wall4 = new Wall(placement.Opposite(), _board.GetCellAt(newPos, placement).Position);

            var dir1 = wall.From.GetDirFor(wall3.From);
            var dir2 = wall2.From.GetDirFor(wall4.From);

            if (!_board.GetCell(wall.From).IsAccessible(dir1) && !_board.GetCell(wall2.From).IsAccessible(dir2))
            {
                var errorMessage = $"wall '{from}' : '{placement}' intersects with a previously added wall";
                throw new WallIntersectsException(errorMessage);
            }

            return new List<IWall> { wall, wall2, wall3, wall4 };
        }

        public IWall CreateAndValidateWall(Vector2 from, Direction dir)
        {
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

            if (NewMoveBlockedByWall(currentPos, newPos))
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

        public void Move(IPlayer player, Movement move)
        {
            ValidateNotNull(player, nameof(player));
            ValidateNotNull(move, nameof(move));

            if (move is AgentMove agentMove)
                MovePlayer(player, agentMove.Dir);

            else if (move is WallPlacement wallMove)
                AddWall(player, wallMove.From, wallMove.Dir);

            else if (move is Vector2 vecMove)
            {
                var dir = CurrentPlayer.CurrentPos.GetDirFor(vecMove);
                MovePlayer(player, dir);
            }
            else throw new Exception($"move type {typeof(Movement).Name} not supported");
        }

        public void UndoMove(IPlayer player, Movement move)
        {
            ValidateNotNull(player, nameof(player));
            ValidateNotNull(move, nameof(move));

            if (move is AgentMove agentMove)
                MovePlayer(player, agentMove.Dir.Opposite());

            else if (move is WallPlacement wallMove)
                RemoveWall(player, wallMove.From, wallMove.Dir);
        }

        public IEnumerable<Movement> GetValidMovesFor(IPlayer player)
        {
            var validMoves = new List<Movement>();

            //movable player positions (at most 4)
            var validPlayerMoves = _board
                .NeighborDirs(player.CurrentPos)
                .Select(d => new AgentMove(d));

            validMoves.AddRange(validPlayerMoves);

            //if player has no wall remaining, we don't need to add in wall moves.
            if (player.NumWalls <= 0)
                return validMoves;

            //all wall pieces that can be placed on the board
            //horizontal walls
            for (int i = 0; i <= _board.Dimension - 3; i++)
                for (int j = 1; j < _board.Dimension - 1; j++)
                {
                    var from = new Vector2(i, j);
                    if (Walls.All(wall => !wall.Intersects(from, Direction.North)))
                        validMoves.Add(new WallPlacement(Direction.North, from));
                }

            //vertical walls
            for (int i = 1; i < _board.Dimension - 1; i++)
                for (int j = 0; j <= _board.Dimension - 3; j++)
                {
                    var from = new Vector2(i, j);
                    if (Walls.All(wall => !wall.Intersects(from, Direction.West)))
                        validMoves.Add(new WallPlacement(Direction.West, from));
                }


            return validMoves;
        }

        public double Evaluate(IPlayer agent)
        {
            var result = _aStar.BestMove(_board, agent);
            var goalDistance = result.Value;
            var wallsLeft = agent.NumWalls;

            var otherAgent = Players.First(p => !p.Equals(agent));
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
