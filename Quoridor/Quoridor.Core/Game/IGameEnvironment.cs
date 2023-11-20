using System.Collections.Generic;

using Quoridor.Core.Utils;
using Quoridor.AI.Interfaces;
using Quoridor.Core.Entities;
using Quoridor.Core.Environment;
using Quoridor.AI.MinimaxAlgorithm;
using ConcurrentCollections;

namespace Quoridor.Core.Game
{
    public interface IGameEnvironment :
        IGame<IPlayer, Movement>,
        INeighbors<Vector2>,
        INeighbors<Movement>,
        IDeepCopy<IGameEnvironment>
    {
        List<IPlayer> Players { get; }
        ConcurrentHashSet<IWall> Walls { get; }
        int ASTAR_COUNT { get; set; }
        int Turn { get; }

        void Initialize();

        void AddPlayer(IPlayer player);
        void MovePlayer(IPlayer player, Direction dir);
        void ChangeTurn();

        void AddWall(IPlayer player, Vector2 from, Direction placement);
        void RemoveWall(IPlayer player, Vector2 from, Direction placement);
    }
}
