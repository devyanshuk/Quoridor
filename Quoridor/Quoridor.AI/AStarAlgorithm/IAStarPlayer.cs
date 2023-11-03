using Quoridor.AI.Interfaces;

namespace Quoridor.AI.AStarAlgorithm
{
    public interface IAStarPlayer<TMove>
        where TMove : Movement
    {
        TMove GetCurrentMove();
        bool IsGoal(TMove move);
        double CalculateHeuristic(TMove move);
    }
}
