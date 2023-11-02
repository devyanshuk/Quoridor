namespace Quoridor.AI.Interfaces
{
    public interface IMove<TMove>
        where TMove : Movement
    {
        void Move(TMove move);
        void UndoMove(TMove move);
    }
}
