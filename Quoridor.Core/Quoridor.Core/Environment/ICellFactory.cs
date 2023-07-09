using System;
using System.Numerics;

namespace Quoridor.Core.Environment
{
    public interface ICellFactory
    {
        ICell Create(Vector2 position);
    }
}
