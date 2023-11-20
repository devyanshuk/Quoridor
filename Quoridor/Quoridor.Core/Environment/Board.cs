using System;
using System.Linq;
using System.Collections.Generic;

using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public class Board : IBoard
    {
        public int Dimension { get; private set; }
        public Cell[,] Cells { get; private set; }

        public Board() { }

        public void SetDimension(int dimension)
        {
            if (dimension < 2)
                throw new Exception($"Dimension '{dimension}'. Invalid.");

            Dimension = dimension;
            Initialize();
        }

        public Cell GetCell(Vector2 vec) => GetCell(vec.X, vec.Y);

        public Cell GetCell(int x, int y) => Cells[x, y];

        public Cell GetCellAt(Vector2 from, Direction dir)
        {
            var newPos = from.GetPosFor(dir);
            return GetCell(newPos);
        }

        public bool WithinBounds(Vector2 pos)
        {
            return pos.X < Dimension && pos.X >= 0 && pos.Y < Dimension && pos.Y >= 0;
        }

        public void Initialize()
        {
            Cells = new Cell[Dimension, Dimension];
            for (int i = 0; i < Dimension; i++)
                for (int j = 0; j < Dimension; j++)
                    Cells[i, j] = new Cell(new Vector2(i, j));
        }

        public IEnumerable<Cell> Neighbors(Cell refCell)
        {
            foreach (var neighborDir in NeighborDirs(refCell))
                yield return GetCell(refCell.Position.GetPosFor(neighborDir));
        }

        public IEnumerable<Direction> NeighborDirs(Cell refCell)
        {
            var x = refCell.Position.X;
            var y = refCell.Position.Y;

            if (y + 1 < Dimension && refCell.IsAccessible(Direction.South))
                yield return Direction.South;

            if (y - 1 >= 0 && refCell.IsAccessible(Direction.North))
                yield return Direction.North;

            if (x + 1 < Dimension && refCell.IsAccessible(Direction.East))
                yield return Direction.East;

            if (x - 1 >= 0 && refCell.IsAccessible(Direction.West))
                yield return Direction.West;
        }

        public IEnumerable<Direction> NeighborDirs(Vector2 pos)
        {
            var cellForPos = GetCell(pos);
            return NeighborDirs(cellForPos);
        }

        public IEnumerable<Vector2> Neighbors(Vector2 pos)
        {
            var cellForPos = GetCell(pos);
            return Neighbors(cellForPos).Select(cell => cell.Position);
        }

        public IEnumerable<Movement> Neighbors(Movement pos)
        {
            var posVec = pos as Vector2;
            return Neighbors(posVec);
        }

        public IBoard DeepCopy()
        {
            var ret = new Board();
            ret.SetDimension(Dimension);
            for (int i = 0; i < Dimension; i++)
                for (int j = 0; j < Dimension; j++)
                    ret.Cells[i, j] = Cells[i, j].DeepCopy();
            return ret;
        }
    }
}
