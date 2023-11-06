using Quoridor.AI.Interfaces;

namespace Quoridor.AI.MinimaxAlgorithm
{
    public interface IGame<TPlayer, TMove> :
            ICurrentPlayer<TPlayer>,
            IValidMoves<TMove>,
            IMove<TMove>,
            IStaticEvaluation,
            ITerminal
        where TMove : Movement
    {
    }
}