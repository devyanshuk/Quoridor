using Quoridor.AI.Interfaces;

namespace Quoridor.AI.MCTS
{
    public interface IMCTSGame<TGame, TMove, TPlayer> :
        ITerminal,
        IDeepCopy<TGame>,
        IOpponent<TPlayer>,
        IValidMoves<TMove>,
        ICurrentPlayer<TPlayer>
    {
    }
}
