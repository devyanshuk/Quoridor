using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public interface IWallFactory
    {
        IWall CreateWall(Direction placement, Vector2 from);
    }
}
