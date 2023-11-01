using System.Collections.Generic;

namespace Quoridor.AI.Interfaces
{
    public interface INeighbors<TVector2D>
        where TVector2D : class, IVector2D
    {
        IEnumerable<TVector2D> Neighbors(TVector2D pos);
    }
}
