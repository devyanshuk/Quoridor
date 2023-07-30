﻿using System;
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

        public Cell GetCell(Vector2 vec) => Cells[vec.X, vec.Y];

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
