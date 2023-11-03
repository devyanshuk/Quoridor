using System;

using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public class Cell : IEquatable<Cell>
    {
        public Vector2 Position { get; private set; }

        /// <summary>
        /// eg. Blocked[0] == true indicates that there's a wall above the cell
        /// </summary>
        public bool[] Blocked { get; set; } = new bool[4];

        public Cell(Vector2 position)
        {
            Position = position;
            for(int i = 0; i < 4; i++)
                Blocked[i] = false;

        }

        public bool IsAccessible(Direction direction)
        {
            return Blocked[(int)direction] == false;
        }

        public void Block(Direction dir)
        {
            Blocked[(int)dir] = true;
        }

        public void Unblock(Direction dir)
        {
            Blocked[(int)dir] = false;
        }

        public bool Equals(Cell other)
        {
            if (other == null) return false;
            return Position.Equals(other.Position);
        }
    }
}
