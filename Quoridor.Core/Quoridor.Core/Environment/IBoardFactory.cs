using System;
namespace Quoridor.Core.Environment
{
    public interface IBoardFactory
    {
        IBoard Create(int dimension);
    }
}
