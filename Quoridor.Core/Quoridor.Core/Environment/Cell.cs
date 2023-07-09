using System;
using System.Collections.Generic;
using System.Numerics;

namespace Quoridor.Core.Environment
{
    public class Cell : ICell
    {
        public Vector2 Position { get; private set; }
        public HashSet<Vector2> Neighbors { get; private set; }

        private readonly IBoard _board;

        public Cell(Vector2 position, IBoard board)
        {
            Position = position;
            _board = board;
            Neighbors = new HashSet<Vector2>();
            SetNeighbors(position);
        }

        public void SetNeighbors(Vector2 position)
        {
            Neighbors.Clear();


         
        }
    }
}
