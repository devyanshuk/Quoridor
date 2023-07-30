using System;

namespace Quoridor.Core.Utils
{
    public class Vector2 : IEquatable<Vector2>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2 Copy()
        {
            return new Vector2(X, Y);
        }

        public Vector2 PositionAt(Direction dir)
        {
            var copy = Copy();
            switch (dir)
            {
                case Direction.North:
                case Direction.South:
                    { copy.X++; break; }
                case Direction.East:
                default:
                    { copy.Y++; break; }
            }
            return copy;
        }

        public static bool operator==(Vector2 first, Vector2 second)
        {
            return first.X == second.X && first.Y == second.Y;
        }

        public static bool operator!=(Vector2 first, Vector2 second)
        {
            return !(first == second);
        }

        public bool Equals(Vector2 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is Vector2 && Equals((Vector2)obj);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
