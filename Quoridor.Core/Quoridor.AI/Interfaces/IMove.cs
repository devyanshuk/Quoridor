namespace Quoridor.AI.Interfaces
{
    public interface IMove<TPlayer, TMove>
        where TMove : Movement
    {
        void Move(TPlayer player, TMove move);
        void UndoMove(TPlayer player, TMove move);
    }
}
