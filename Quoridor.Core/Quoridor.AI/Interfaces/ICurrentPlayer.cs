using System;

namespace Quoridor.AI.Interfaces
{
    public interface ICurrentPlayer<TPlayer>
        where TPlayer : class, IEquatable<TPlayer>
    {
        TPlayer CurrentPlayer { get; }
    }
}
