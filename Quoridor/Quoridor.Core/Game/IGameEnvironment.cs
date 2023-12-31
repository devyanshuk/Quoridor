﻿using System.Collections.Generic;

using Quoridor.AI.MCTS;
using Quoridor.Core.Utils;
using ConcurrentCollections;
using Quoridor.AI.Interfaces;
using Quoridor.Core.Entities;
using Quoridor.Core.Environment;
using Quoridor.AI.MinimaxAlgorithm;
using System;

namespace Quoridor.Core.Game
{
    public interface IGameEnvironment :
        IMinimaxGame<IPlayer, Movement>,
        IMCTSGame<IGameEnvironment, Movement, IPlayer>,
        IRandomizableMoves<Movement>,
        INeighbors<Vector2>,
        INeighbors<Movement>,
        IDeepCopy<IGameEnvironment>
    {
        EventHandler OnMoveDone { get; set; }

        List<IPlayer> Players { get; }
        ConcurrentHashSet<IWall> Walls { get; }
        int Turn { get; }

        void Initialize();

        void AddPlayer(IPlayer player);
        void MovePlayer(IPlayer player, Direction dir);
        void InitAndAddPlayers(int numPlayers, int numWalls);
        void ChangeTurn();

        void AddWall(IPlayer player, Vector2 from, Direction placement);
        void RemoveWall(IPlayer player, Vector2 from, Direction placement);

        IEnumerable<Movement> GetWalkableNeighbors(IPlayer player);
    }
}
