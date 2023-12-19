namespace Quoridor.AI.Interfaces
{
    public interface IPlayer<TPlayer>
    {
        TPlayer CurrentPlayer { get; }
    }
}
