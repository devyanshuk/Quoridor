using Quoridor.AI.Interfaces;

namespace Quoridor.AI.AStarAlgorithm
{
    public interface IAStarPlayer<TMove>
    {
        TMove GetCurrentMove();
        bool IsGoal(TMove move);
        double CalculateHeuristic(TMove move);
    }
}
