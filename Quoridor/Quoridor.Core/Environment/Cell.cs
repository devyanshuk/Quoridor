using System;

using Quoridor.Core.Utils;
using Quoridor.AI.Interfaces;

namespace Quoridor.Core.Environment
{
    public class Cell : IEquatable<Cell>, IDeepCopy<Cell>
    {
        public Vector2 Position { get; private set; }

        /// <summary>
        /// eg. Blocked[0] == true indicates that there's a wall above the cell
        /// </summary>
        public bool[] Blocked { get; set; } = new bool[4];

        private readonly object _lock = new object();

        public Cell(Vector2 position)
        {
            Position = position;
            for(int i = 0; i < 4; i++)
                Blocked[i] = false;
        }

        public bool IsAccessible(Direction direction) => !Blocked[(int)direction];

        public void Block(Direction dir)
        {
            lock(_lock)
                Blocked[(int)dir] = true;
        }

        public void Unblock(Direction dir)
        {
            lock(_lock)
                Blocked[(int)dir] = false;
        }

        public bool Equals(Cell other)
        {
            if (other == null) return false;
            return Position.Equals(other.Position);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is Cell cell) return Equals(cell);
            return false;
        }

        public Cell DeepCopy()
        {
            return new Cell(Position.Copy())
            {
                //Blocked[] is an array of bool struct so shallow copy is fine
                Blocked = Blocked.Clone() as bool[]
            };
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position);
        }
    }
}
