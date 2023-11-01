using Quoridor.AI.Interfaces;

namespace Quoridor.AI.AStarAlgorithm
{
    public interface IAStarPlayer<TVector2D> : IPosition<TVector2D>
        where TVector2D : class, IVector2D
    {
        IsGoal<TVector2D> IsGoalCell { get; set; }
        H_n<TVector2D> ManhattanHeuristicFn { get; set; }
    }
}
