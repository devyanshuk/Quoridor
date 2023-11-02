namespace Quoridor.AI.Interfaces
{
    public interface IStaticEvaluation<TPlayer>
    {
        public double Evaluate(TPlayer player);
    }
}
