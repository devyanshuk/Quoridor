﻿namespace Quoridor.Core.Game
{
    public interface IGameFactory
    {
        IGameEnvironment CreateGameEnvironment(int numPlayers, int numWalls);

        void Release(IGameEnvironment environment);
    }
}
