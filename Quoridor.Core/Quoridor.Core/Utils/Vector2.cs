using System;
namespace Quoridor.Core.Utils
{
    public struct Vector2 : IEquatable<Vector2>
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
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
            throw new NotImplementedException();
        }
    }
}
