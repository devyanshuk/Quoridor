using System;

using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public class Wall : IWall
    {
        public Direction Placement { get; private set; }
        public Vector2 From { get; private set; }

        public Wall(Direction placement, Vector2 from)
        {
            Placement = placement;
            From = from;
        }

        public Wall() { }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Wall)) return false;
            var wall = obj as Wall;
            return wall.Placement == Placement && wall.From == From;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Placement, From);
        }
    }
}
