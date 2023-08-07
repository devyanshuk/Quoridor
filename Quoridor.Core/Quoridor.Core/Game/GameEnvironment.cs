using System;
using System.Linq;
using System.Collections.Generic;

using Quoridor.Core.Utils;
using Quoridor.Core.Entities;
using Quoridor.Common.Logging;
using Quoridor.Core.Extensions;
using Quoridor.Core.Environment;
using Quoridor.Core.Utils.CustomExceptions;

namespace Quoridor.Core.Game
{
    public class GameEnvironment : IGameEnvironment
    {
        private readonly IBoard _board;

        public int Turn { get; private set; }
        public List<IPlayer> Players { get; private set; }
        public HashSet<IWall> Walls { get; private set; } = new HashSet<IWall>();

        private readonly ILogger _log = Logger.InstanceFor<GameEnvironment>();

        public GameEnvironment(
            IBoard board)
        {
            _board = board;
        }

        public void Initialize()
        {
            Turn = 0;
            _board.Initialize();
        }

        public void AddPlayer(IPlayer player)
        {
            if (Players == null)
            {
                Players = new List<IPlayer>();
            }
            Players.Add(player);
            _log.Info($@"Adding player '{player.Id}'. Available players: '{
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
                return Players?[Turn];
            }
        }

        public void MovePlayer(Direction dir)
        {
            if (Players == null)
                throw new Exception($"No player registered. Call the {nameof(AddPlayer)} method to register");

            _log.Info($"Moving player '{CurrentPlayer.Id}' '{dir}'...");

            var newPos = TryMove(CurrentPlayer.CurrentPos, dir);
            CurrentPlayer.CurrentPos = newPos;

            _log.Info($"Moved player '{CurrentPlayer.Id}' to '{newPos}'");
        }


        public void AddWall(Vector2 from, Direction placement)
        {
            _log.Info($"Attempting to add '{placement}ern' wall from '{from}'");

            if (!_board.WithinBounds(from))
            {
                var errorMessage = $"{placement}ern wall from '{from}' could not be added. Invalid dimension.";
                _log.Error(errorMessage);
                throw new InvalidWallException(errorMessage);
            }

            var walls = GetWallsForAffectedCells(from, placement);
            if (walls.All(w => _board.GetCell(w.From).IsAccessible(w.Placement)))
            {
                _log.Info($"Wall validity check complete. Adding wall '{from}': '{placement}' to the board");
                foreach(var wall in walls)
                {
                    _board.GetCell(wall.From).AddWall(wall);
                    _log.Info($"Cell {wall.From} is now inaccessible on the '{wall.Placement}ern' side");
                }
            }
            else throw new WallAlreadyPresentException($"{placement}ern wall from '{from}' already present");

            Walls.Add(walls.First());

            _log.Info($"Successfully added '{placement}ern' wall from '{from}'");

            CurrentPlayer?.DecreaseWallCount();
        }

        public void RemoveWall(Vector2 from, Direction placement)
        {
            _log.Info($"Attempting to remove '{placement}ern' wall from '{from}'");

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

            Walls.Remove(walls.First());

            _log.Info($"Successfully removed '{placement}ern' wall from '{from}'");

            CurrentPlayer?.IncreaseWallCount();
        }

        public bool NewMoveBlockedByWall(Vector2 currPos, Vector2 newPos)
        {
            var neighbors = _board.Neighbors(_board.GetCell(currPos));
            return neighbors.All(n => !n.Position.Equals(newPos));
        }

        public IEnumerable<IWall> GetWallsForAffectedCells(Vector2 from, Direction placement)
        {
            var wall = CreateAndValidateWall(from, placement);
            var wall2 = CreateAndValidateWall(_board.GetCellAt(from, placement).Position, placement.Opposite());

            var newPos = from.Copy();
            if (wall.IsHorizontal()) newPos.X++;
            else newPos.Y++;

            var wall3 = CreateAndValidateWall(newPos, placement);
            var wall4 = CreateAndValidateWall(_board.GetCellAt(newPos, placement).Position, placement.Opposite());

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

        private Vector2 TryMove(Vector2 currentPos, Direction dir)
        {
            var newPos = currentPos.GetPosFor(dir);
            var player = Players[Turn];

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
                return TryMove(newPos, dir);
            }
            _log.Info($"No player found in '{newPos}'. Moving player '{player.Id}' from '{player.CurrentPos}' to '{newPos}'");
            return newPos;
        }
    }
}
