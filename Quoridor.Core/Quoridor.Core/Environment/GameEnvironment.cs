using System;

using Quoridor.Core.Utils;
using Quoridor.Core.Extensions;
using Quoridor.Core.Utils.CustomExceptions;

namespace Quoridor.Core.Environment
{
    public class GameEnvironment : IGameEnvironment
    {
        private readonly IBoard _board;
        private readonly IWallFactory _wallFactory;

        public GameEnvironment(
            IBoard board,
            IWallFactory wallFactory)
        {
            _board = board;
            _wallFactory = wallFactory;
        }


        public void AddWall(Vector2 from, Direction placement)
        {
            if (from.X >= _board.Dimension || from.X < 0 || from.Y >= _board.Dimension || from.Y < 0)
                throw new InvalidWallException($"{placement}ern wall from '{from}' not possible. Invalid dimension.");

            IWall wall1;
            IWall wall2;

            try
            {
                wall1 = CreateAndValidateWall(from, placement);
                wall2 = CreateAndValidateWall(_board.GetCellAt(from, placement).Position, placement.Opposite());
            }
            catch
            {
                throw new InvalidWallException($"{placement}ern wall from '{from} not possible.'");
            }

            var cell1 = _board.GetCell(from);
            var cell2 = _board.GetCell(wall2.From);

            if (cell1.IsAccessible(placement) && cell2.IsAccessible(wall2.Placement))
            {
                cell1.AddWall(wall1);
                cell2.AddWall(wall2);
            }
            else
                throw new WallAlreadyPresentException($"{placement} wall from '{from}' already present");
        }

        public IWall CreateAndValidateWall(Vector2 from, Direction dir)
        {
            var wall = _wallFactory.CreateWall(dir, from);

            if ((wall.From.X == 0 && wall.Placement.Equals(Direction.West))
                || (wall.From.Y == 0 && wall.Placement.Equals(Direction.North))
                || (wall.From.X == _board.Dimension - 1 && wall.Placement.Equals(Direction.East))
                || (wall.From.Y == _board.Dimension - 1 && wall.Placement.Equals(Direction.South))) {
                throw new Exception();
            }

            return wall;
        }


    }
}
