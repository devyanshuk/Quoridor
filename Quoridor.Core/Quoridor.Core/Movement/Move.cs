using Quoridor.Core.Utils;

namespace Quoridor.Core.Movement
{
    public class Move
    {
        public Direction Dir { get; set; }

        public Move(Direction dir)
        {
            Dir = dir;
        }
    }
}
