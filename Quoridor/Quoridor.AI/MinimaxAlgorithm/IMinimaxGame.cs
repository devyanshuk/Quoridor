using Quoridor.AI.Interfaces;

namespace Quoridor.AI.MinimaxAlgorithm
{
    public interface IMinimaxGame<TPlayer, TMove> :
        ITerminal,
        IMove<TMove>,
        IStaticEvaluation,
        IValidMoves<TMove>,
        ICurrentPlayer<TPlayer>
    {
    }
}