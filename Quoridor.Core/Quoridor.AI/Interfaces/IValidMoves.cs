using System;
using System.Collections.Generic;

namespace Quoridor.AI.Interfaces
{
    public interface IValidMoves<TMove, TPlayer>
        where TMove : class
        where TPlayer : class, IEquatable<TPlayer>
    {
        IEnumerable<TMove> GetValidMovesFor(TPlayer player);
    }
}
