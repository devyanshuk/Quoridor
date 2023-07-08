using System;
namespace Quoridor.Core.Environment
{
    public interface IBoard
    {
        int Dimension { get; }
        Cell[,] Cells { get; }
    }
}
