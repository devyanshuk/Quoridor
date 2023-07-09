using System;
using System.Collections.Generic;

using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public class Board : IBoard
    {
        public int Dimension { get; private set; }
        public Cell[,] Cells { get; private set; }

        private readonly IWallFactory _wallFactory;

        public Board(IWallFactory wallFactory)
        {
            _wallFactory = wallFactory;
        }

        public void SetDimension(int dimension)
        {
            if (dimension < 2)
                throw new Exception($"Dimension '{dimension}'. Invalid.");

            Dimension = dimension;
            Initialize();
        }


        public void Initialize()
        {
            Cells = new Cell[Dimension, Dimension];
            for (int i = 0; i < Dimension; i++)
                for (int j = 0; j < Dimension; j++)
                    Cells[i, j] = new Cell(new Vector2(i, j));
        }

        public void AddWall(Vector2 from, Vector2 to)
        {
            if (from.X != to.X && from.Y != to.Y)
                throw new Exception($"{from} -> {to} : Not a valid wall");
            var orientation = from.X == to.X ? Placement.Horizontal : Placement.Vertical;
            var wall = _wallFactory.Create(orientation, from, to);

        }

        public IEnumerable<Cell> Neighbors(Cell refCell)
        {
            var x = refCell.Position.X;
            var y = refCell.Position.Y;

            if (y + 1 < Dimension && refCell.IsAccessible(Direction.South))
                yield return Cells[x, y + 1];

            if (y - 1 >= 0 && refCell.IsAccessible(Direction.North))
                yield return Cells[x, y - 1];

            if (x + 1 < Dimension && refCell.IsAccessible(Direction.East))
                yield return Cells[x + 1, y];

            if (x - 1 > 0 && refCell.IsAccessible(Direction.West))
                yield return Cells[x - 1, y];
        }
    }
}
