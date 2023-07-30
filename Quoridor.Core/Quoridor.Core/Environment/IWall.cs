using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public interface IWall
    {
        Direction Placement { get; }
        Vector2 From { get; }

        bool IsHorizontal();
        bool IsVertical();
    }
}
