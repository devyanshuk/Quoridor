using Quoridor.AI.Interfaces;

namespace Quoridor.AI.MCTS
{
    public interface IMCTSGame<TGame, TMove, TPlayer> :
        ITerminal,
        IMove<TMove>,
        IWinner<TPlayer>,
        IDeepCopy<TGame>,
        IOpponent<TPlayer>,
        IValidMoves<TMove>,
        IPlayer<TPlayer>
    {
    }
}
