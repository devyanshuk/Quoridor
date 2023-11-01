using System;
using Quoridor.Core.Utils;
using Quoridor.AI.AStarAlgorithm;

namespace Quoridor.Core.Entities
{
    public interface IPlayer : IAStarPlayer<Vector2>, IEquatable<IPlayer>
    {
        char Id { get; }
        int NumWalls { get; set; }
        Vector2 StartPos { get; }
        int CurrX { get; }
        int CurrY { get; }

        void Move(Vector2 newPos);
        void DecreaseWallCount();
        void IncreaseWallCount();
    }
}
