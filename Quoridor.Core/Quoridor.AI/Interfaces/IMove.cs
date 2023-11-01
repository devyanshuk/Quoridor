namespace Quoridor.AI.Interfaces
{
    public interface IMove<TMove>
        where TMove : class
    {
        void Move(TMove move);
        void UndoMove(TMove move);
    }
}
