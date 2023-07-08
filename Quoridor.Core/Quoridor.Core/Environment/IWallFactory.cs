using System;

namespace Quoridor.Core.Environment
{
    public interface IWallFactory
    {
        IWall Create(Placement placement);
    }
}
