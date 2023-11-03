using System;
namespace Quoridor.AI.Interfaces
{
    public interface ITerminal
    {
        bool HasFinished { get; }
    }
}
