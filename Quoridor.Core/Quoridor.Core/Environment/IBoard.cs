using System.Collections.Generic;

using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public interface IBoard
    {
        int Dimension { get; }
        Cell[,] Cells { get; }

        void Initialize();
        void SetDimension(int dimension);
        bool WithinBounds(Vector2 pos);

        Cell GetCell(Vector2 vec);
        Cell GetCellAt(Vector2 from, Direction dir);

        IEnumerable<Cell> Neighbors(Cell refCell);
    }
}
