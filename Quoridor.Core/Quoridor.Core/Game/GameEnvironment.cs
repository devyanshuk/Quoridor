using System.Linq;
using System.Collections.Generic;

using Quoridor.Core.Utils;
using Quoridor.Core.Entities;
using Quoridor.Core.Extensions;
using Quoridor.Core.Environment;
using Quoridor.Core.Utils.CustomExceptions;

namespace Quoridor.Core.Game
{
    public class GameEnvironment : IGameEnvironment
    {
        private readonly IBoard _board;

        public int Turn { get; private set; }
        public List<IPlayer> Players { get; } = new List<IPlayer>();
        public HashSet<IWall> Walls { get; private set; } = new HashSet<IWall>();

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
            Players.Add(player);
        }

        public void ChangeTurn()
        {
            Turn = (Turn + 1) % Players.Count;
        }

        public IPlayer CurrentPlayer
        {
            get
            {
                return Players[Turn];
            }
        }

        public void MovePlayer(Direction dir)
        {
            var player = Players[Turn];
            var newPos = player.CurrentPos.GetPosFor(dir);

            var jump = Players.FirstOrDefault(p => p.CurrentPos.Equals(newPos)) != null;

            if (!_board.WithinBounds(newPos))
                throw new InvalidAgentMoveException($"player '{player.Id}' cannot move to '{newPos}'. Invalid move position");

            if (!NewMoveBlockedByWall(player.CurrentPos, newPos))
            {
                var canMove = true;
                if (jump)
                {
                    var jumpPos = newPos.GetPosFor(dir);
                    if (NewMoveBlockedByWall(newPos, jumpPos))
                        canMove = false;
                    else
                        newPos = jumpPos;
                }
                if (canMove)
                {
                    player.Move(newPos);
                    return;
                }
            }
            throw new NewMoveBlockedByWallException($"player '{player.Id}' cannot move to '{newPos}' since it's blocked by a wall");
        }

        public void AddWall(Vector2 from, Direction placement)
        {
            if (!_board.WithinBounds(from))
                throw new InvalidWallException($"{placement}ern wall from '{from}' could not be added. Invalid dimension.");

            var walls = GetWallsForAffectedCells(from, placement);
            if (walls.All(w => _board.GetCell(w.From).IsAccessible(w.Placement)))
            {
                foreach(var wall in walls)
                {
                    _board.GetCell(wall.From).AddWall(wall);
                }
            }
            else throw new WallAlreadyPresentException($"{placement}ern wall from '{from}' already present");

            Walls.Add(walls.First());
            Players[Turn].NumWalls--;
        }

        public void RemoveWall(Vector2 from, Direction placement)
        {
            if (!_board.WithinBounds(from))
                throw new InvalidWallException($"{placement}ern wall from '{from}' could not be removed. Invalid dimension");

            var walls = GetWallsForAffectedCells(from, placement);
            if (walls.All(w => !_board.GetCell(w.From).IsAccessible(w.Placement)))
            {
                foreach(var wall in walls)
                {
                    _board.GetCell(wall.From).RemoveWall(wall);
                }
            }
            else throw new WallNotPresentException($"{placement}ern wall from '{from} not present'");

            Walls.Remove(walls.First());
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
                throw new WallIntersectsException($"wall '{from}' : '{placement}' intersects with a previously added wall");

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
                throw new InvalidWallException($"{dir}ern wall from '{from} not possible.'");
            }
            return wall;
        }
    }
}
