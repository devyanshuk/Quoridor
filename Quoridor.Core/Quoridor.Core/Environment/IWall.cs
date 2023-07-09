using System;
using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public interface IWall
    {
        Placement Placement { get; }
    }
}
