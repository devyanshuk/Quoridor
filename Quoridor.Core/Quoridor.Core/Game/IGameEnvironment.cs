﻿using System.Collections.Generic;

using Quoridor.Core.Utils;
using Quoridor.Core.Entities;

namespace Quoridor.Core.Game
{
    public interface IGameEnvironment
    {
        int Turn { get; }
        List<IPlayer> Players { get; }

        void Initialize();

        void AddPlayer(IPlayer player);
        void MovePlayer(Direction dir);
        void ChangeTurn();

        IPlayer CurrentPlayer { get; }

        void AddWall(Vector2 from, Direction placement);
        void RemoveWall(Vector2 from, Direction placement);
    }
}
