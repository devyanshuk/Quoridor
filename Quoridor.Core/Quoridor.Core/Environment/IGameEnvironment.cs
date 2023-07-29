using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public interface IGameEnvironment
    {
        void AddWall(Vector2 from, Direction placement);
    }
}
