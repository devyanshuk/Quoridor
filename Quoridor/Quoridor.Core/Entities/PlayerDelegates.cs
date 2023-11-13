namespace Quoridor.Core.Entities
{
    public delegate bool IsGoal<TVector2D>(TVector2D pos)
    where TVector2D : class;

    public delegate double H_n<TVector2D>(TVector2D pos)
        where TVector2D : class;
}
