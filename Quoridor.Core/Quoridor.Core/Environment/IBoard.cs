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
        void AddWall(Vector2 from, Vector2 to);
        IEnumerable<Cell> Neighbors(Cell refCell);
    }
}
