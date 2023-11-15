namespace Quoridor.AI.Interfaces
{
    public interface IMove<TMove>
    {
        void Move(TMove move);
        void UndoMove(TMove move);
    }
}
