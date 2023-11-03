using System;

namespace Quoridor.AI.Interfaces
{
    public interface ICurrentPlayer<TPlayer>
    {
        TPlayer CurrentPlayer { get; }
    }
}
