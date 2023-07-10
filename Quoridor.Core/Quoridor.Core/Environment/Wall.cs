using System;

using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public class Wall : IWall
    {
        public Placement Placement { get; private set; }
        public Vector2 From { get; private set; }
        public Vector2 To { get; private set; }


        public Wall(Placement placement, Vector2 from, Vector2 to)
        {
            Placement = placement;
            From = from;
            To = to;
        }

        public Wall() { }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Wall)) return false;
            var wall = obj as Wall;
            return wall.Placement == Placement &&
                wall.From == From &&
                wall.To == To;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Placement, From, To);
        }
    }
}
