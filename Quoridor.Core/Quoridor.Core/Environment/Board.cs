using System;
namespace Quoridor.Core.Environment
{
    public class Board : IBoard
    {
        public int Dimension { get; private set; }
        public Cell[,] Cells { get; private set; }

        private readonly IWallFactory _wallFactory;

        public Board(int dimension, IWallFactory wallFactory)
        {
            this.Dimension = dimension;
            _wallFactory = wallFactory;
        }
    }
}
