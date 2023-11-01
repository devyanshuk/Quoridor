using System;
using System.Collections.Generic;

namespace Quoridor.AI.Interfaces
{
    public interface IGame<TPlayer, TMove> :
            ICurrentPlayer<TPlayer>,
            IValidMoves<TMove, TPlayer>,
            IMove<TMove>,
            IStaticEvaluation<TPlayer>,
            ITerminal

        where TPlayer : class, IEquatable<TPlayer>
        where TMove : class
    {
    }
}