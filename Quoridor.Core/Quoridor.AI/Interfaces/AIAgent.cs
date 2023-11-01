using System;

namespace Quoridor.AI.Interfaces
{
    public abstract class AIAgent<TMove, TGameView, TPlayer>
        where TMove : class
        where TPlayer : class, IEquatable<TPlayer>
    {
        public abstract string Name { get; }
        public abstract TMove BestMove(TGameView gameView, TPlayer player);
    }
}
