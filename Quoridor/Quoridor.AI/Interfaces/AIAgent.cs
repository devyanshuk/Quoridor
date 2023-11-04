namespace Quoridor.AI.Interfaces
{
    public abstract class AIStrategy<TMove, TGame, TPlayer>
        where TMove : Movement
    {
        public abstract string Name { get; }
        public abstract AIStrategyResult<TMove> BestMove(TGame game, TPlayer player);
    }

    public class AIStrategyResult<TMove>
        where TMove : Movement
    {
        public double Value { get; set; }
        public TMove BestMove { get; set; }

        public override string ToString()
        {
            return $"BestMove : {BestMove}, Value : {Value}";
        }
    }
}
