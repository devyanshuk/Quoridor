using System;
using System.Numerics;

namespace Quoridor.Core.Environment
{
    public class Board : IBoard
    {
        public int Dimension { get; private set; }
        public ICell[,] Cells { get; private set; }

        private readonly IWallFactory _wallFactory;
        private readonly ICellFactory _cellFactory;

        public Board(IWallFactory wallFactory, ICellFactory cellFactory)
        {
            _wallFactory = wallFactory;
            _cellFactory = cellFactory;
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
                    Cells[i, j] = _cellFactory.Create(new Vector2(i, j));
        }
    }
}
