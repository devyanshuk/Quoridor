namespace Quoridor.AI.Interfaces
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