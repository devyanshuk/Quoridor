using System;
using System.Numerics;

namespace Quoridor.Core.Environment
{
    public interface IWallFactory
    {
        IWall Create(Placement placement, Vector2 from, Vector2 to);
    }
}
