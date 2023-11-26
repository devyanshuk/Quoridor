namespace Quoridor.AI.Interfaces
{
    public interface IOpponent<TPlayer>
    {
        TPlayer Opponent { get; }
    }
}
