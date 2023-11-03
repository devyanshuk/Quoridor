using System.Collections.Generic;

namespace Quoridor.AI.Interfaces
{
    public interface IValidMoves<TMove>
        where TMove : Movement
    {
        IEnumerable<TMove> GetValidMoves();
    }
}
