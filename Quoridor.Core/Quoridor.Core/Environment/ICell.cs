using System;
using System.Collections.Generic;

using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public interface ICell
    {
        Vector2 Position { get; }

        IEnumerable<ICell> GetNeighbors(ICell refCell);
    }
}
