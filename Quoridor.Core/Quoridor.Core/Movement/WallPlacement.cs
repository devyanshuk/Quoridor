using Quoridor.Core.Utils;

namespace Quoridor.Core.Movement
{
    public class WallPlacement : Move
    {
        public Vector2 From { get; set; }

        public WallPlacement(Direction dir, Vector2 from) : base(dir)
        {
            From = from;
        }
    }
}
