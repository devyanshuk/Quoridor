using System.Collections.Generic;

namespace Quoridor.AI.Interfaces
{
    public interface IValidMoves<TMove, TPlayer>
        where TMove : Movement
    {
        IEnumerable<TMove> GetValidMovesFor(TPlayer player);
    }
}
