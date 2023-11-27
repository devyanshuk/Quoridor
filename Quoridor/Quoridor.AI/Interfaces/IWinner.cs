namespace Quoridor.AI.Interfaces
{
    public interface IWinner<TPlayer>
    {
        TPlayer Winner { get; }
    }
}
