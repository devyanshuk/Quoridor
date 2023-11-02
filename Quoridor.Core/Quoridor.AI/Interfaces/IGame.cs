namespace Quoridor.AI.Interfaces
{
    public interface IGame<TPlayer, TMove> :
            ICurrentPlayer<TPlayer>,
            IValidMoves<TMove, TPlayer>,
            IMove<TPlayer, TMove>,
            IStaticEvaluation<TPlayer>,
            ITerminal
        where TMove : Movement
    {
    }
}