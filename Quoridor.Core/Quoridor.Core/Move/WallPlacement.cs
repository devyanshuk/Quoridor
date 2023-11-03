using System;

using Quoridor.AI.Interfaces;
using Quoridor.Core.Utils;

namespace Quoridor.Core.Move
{
    public sealed class WallPlacement : Movement, IEquatable<WallPlacement>
    {
        public Vector2 From { get; private set; }
        public Direction Dir { get; private set; }

        public WallPlacement(Direction dir, Vector2 from)
        {
            Dir = dir;
            From = from;
        }

        public bool Equals(WallPlacement other)
        {
            if (other is null) return false;
            return other.From.Equals(From) && other.Dir.Equals(Dir);
        }

        public override string ToString()
        {
            return $"{Dir}ern wall from {From}";
        }
    }
}
