using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public class Cell
    {
        public Vector2 Position { get; private set; }

        public Wall[] Walls { get; set; } = new Wall[4];

        public Cell(Vector2 position)
        {
            Position = position;
        }

        public bool IsAccessible(Direction direction)
        {
            return Walls[(int)direction] == null;
        }
    }
}
