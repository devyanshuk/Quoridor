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
    }
}
