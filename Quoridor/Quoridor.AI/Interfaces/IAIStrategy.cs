namespace Quoridor.AI.Interfaces
{
    public interface IAIStrategy<TMove, TGame, TPlayer>
    {
        string Name { get; }
        AIStrategyResult<TMove> BestMove(TGame game, TPlayer player);
    }

    public class AIStrategyResult<TMove>
    {
        public double Value { get; set; }
        public TMove BestMove { get; set; }

        public override string ToString()
        {
            return $"BestMove : {BestMove}, Value : {Value}";
        }
    }
}
