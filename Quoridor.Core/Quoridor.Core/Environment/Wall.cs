using System;

namespace Quoridor.Core.Environment
{
    public class Wall : IWall
    {
        public Placement Placement { get; private set; }

        public Wall(Placement placement)
        {
            this.Placement = placement;
        }
    }
}
