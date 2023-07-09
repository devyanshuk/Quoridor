using System;
using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public interface ICellFactory
    {
        ICell Create(Vector2 position);
    }
}
