using System.Collections.Generic;

namespace Quoridor.AI.Interfaces
{
    public interface INeighbors<TMove>
    {
        IEnumerable<TMove> Neighbors(TMove pos);
    }
}
