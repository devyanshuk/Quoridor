using Quoridor.AI.Interfaces;

namespace Quoridor.AI.AStarAlgorithm
{
    public delegate bool IsGoal<TVector2D>(TVector2D pos)
        where TVector2D : class, IVector2D;

    public delegate double H_n<TVector2D>(TVector2D pos)
        where TVector2D : class, IVector2D;
}
