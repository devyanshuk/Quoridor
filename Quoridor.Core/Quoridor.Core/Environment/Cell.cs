using System;

using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public class Cell : IEquatable<Cell>
    {
        public Vector2 Position { get; private set; }

        /// <summary>
        /// eg. Walls[0] != null indicates that there's a wall above the cell
        /// </summary>
        public IWall[] Walls { get; set; } = new IWall[4];

        public Cell(Vector2 position)
        {
            Position = position;
        }

        public bool IsAccessible(Direction direction)
        {
            return Walls[(int)direction] == null;
        }

        public bool AddWall(IWall wall)
        {
            if (!IsAccessible(wall.Placement))
                return false;

            Walls[(int)wall.Placement] = wall;
            return true;
        }

        public bool RemoveWall(IWall wall)
        {
            if (IsAccessible(wall.Placement))
                return false;

            Walls[(int)wall.Placement] = null;
            return true;
        }

        public bool Equals(Cell other)
        {
            if (other == null) return false;
            return Position.Equals(other.Position) && Walls.Equals(other.Walls);
        }
    }
}
