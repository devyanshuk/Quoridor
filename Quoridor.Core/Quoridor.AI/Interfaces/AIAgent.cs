namespace Quoridor.AI.Interfaces
{
    public abstract class AIStrategy<TMove, TGameView, TPlayer>
        where TMove : Movement
    {
        public abstract string Name { get; }
        public abstract AIStrategyResult<TMove> BestMove(TGameView gameView, TPlayer player);
    }

    public class AIStrategyResult<TMove>
        where TMove : Movement
    {
        public double Value { get; set; }
        public TMove BestMove { get; set; }
    }
}
