using System;

using Quoridor.Core.Utils;
using Quoridor.AI.Interfaces;
using Quoridor.AI.AStarAlgorithm;

namespace Quoridor.Core.Entities
{
    public interface IPlayer :
        IAStarPlayer<Movement>,
        IAStarPlayer<Vector2>,
        IDeepCopy<IPlayer>,
        IEquatable<IPlayer>
    {
        char Id { get; }
        int NumWalls { get; set; }
        Vector2 StartPos { get; }
        Vector2 CurrentPos { get; set; }
        int CurrX { get; }
        int CurrY { get; }

        IsGoal<Vector2> IsGoalMove { get; set; }
        H_n<Vector2> ManhattanHeuristicFn { get; set; }

        void Initialize();

        bool Won();

        void Move(Vector2 newPos);
        void DecreaseWallCount();
        void IncreaseWallCount();
    }
}
