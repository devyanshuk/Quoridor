using System.Collections.Generic;

using Quoridor.AI.MCTS;
using Quoridor.Core.Utils;
using ConcurrentCollections;
using Quoridor.AI.Interfaces;
using Quoridor.Core.Entities;
using Quoridor.Core.Environment;
using Quoridor.AI.MinimaxAlgorithm;

namespace Quoridor.Core.Game
{
    public interface IGameEnvironment :
        IMinimaxGame<IPlayer, Movement>,
        IMCTSGame<IGameEnvironment, Movement, IPlayer>,
        INonPlayerMovableMove<Movement>,
        INeighbors<Vector2>,
        INeighbors<Movement>,
        IDeepCopy<IGameEnvironment>
    {
        List<IPlayer> Players { get; }
        ConcurrentHashSet<IWall> Walls { get; }
        int Turn { get; }

        void Initialize();

        void AddPlayer(IPlayer player);
        void MovePlayer(IPlayer player, Direction dir);
        void ChangeTurn();

        void AddWall(IPlayer player, Vector2 from, Direction placement);
        void RemoveWall(IPlayer player, Vector2 from, Direction placement);
    }
}
