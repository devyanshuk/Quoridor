using System.Collections.Generic;

namespace Quoridor.AI.Interfaces
{
    public interface IValidMoves<TMove>
    {
        IEnumerable<TMove> GetValidMoves();
    }
}
