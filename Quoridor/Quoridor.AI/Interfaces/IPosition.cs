namespace Quoridor.AI.Interfaces
{
    public interface IPosition<TPosition>
        where TPosition : class, IVector2D
    {
        TPosition CurrentPos { get; set; }
    }

    public interface IVector2D
    {
        int X { get; set; }
        int Y { get; set; }
    }
}
