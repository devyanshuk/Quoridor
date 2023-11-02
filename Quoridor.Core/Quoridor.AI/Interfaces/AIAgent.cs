namespace Quoridor.AI.Interfaces
{
    public abstract class AIAgent<TMove, TGameView, TPlayer>
        where TMove : Movement
    {
        public abstract string Name { get; }
        public abstract TMove BestMove(TGameView gameView, TPlayer player);
    }
}
