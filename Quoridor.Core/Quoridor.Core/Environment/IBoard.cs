using System;
namespace Quoridor.Core.Environment
{
    public interface IBoard
    {
        int Dimension { get; }
        ICell[,] Cells { get; }

        void Initialize();
        void SetDimension(int dimension);
    }
}
