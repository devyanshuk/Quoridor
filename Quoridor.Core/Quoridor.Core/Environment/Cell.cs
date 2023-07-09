using System;
using System.Collections.Generic;
using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public class Cell : ICell
    {
        public Vector2 Position { get; private set; }

        private readonly IBoard _board;

        public Cell(Vector2 position, IBoard board)
        {
            Position = position;
            _board = board;
        }

        public IEnumerable<ICell> GetNeighbors(ICell refCell)
        {
            var refPos = refCell.Position;

            if (refPos.Y + 1 < _board.Dimension)
                yield return _board.Cells[refPos.X, refPos.Y + 1];

            if (refPos.Y - 1 >= 0)
                yield return _board.Cells[refPos.X, refPos.Y - 1];

            if (refPos.X + 1 < _board.Dimension)
                yield return _board.Cells[refPos.X + 1, refPos.Y];

            if (refPos.X - 1 > 0)
                yield return _board.Cells[refPos.X - 1, refPos.Y];
        }
    }
}
