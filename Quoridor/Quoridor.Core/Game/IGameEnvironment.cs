using System.Collections.Generic;

using Quoridor.Core.Utils;
using Quoridor.AI.Interfaces;
using Quoridor.Core.Entities;
using Quoridor.Core.Environment;

namespace Quoridor.Core.Game
{
    public interface IGameEnvironment :
        IGame<IPlayer, Movement>,
        INeighbors<Vector2>,
        INeighbors<Movement>
    {
        List<IPlayer> Players { get; }
        HashSet<IWall> Walls { get; }
        int Turn { get; }

        void Initialize();

        void AddPlayer(IPlayer player);
        void MovePlayer(IPlayer player, Direction dir);
        void ChangeTurn();

        void AddWall(IPlayer player, Vector2 from, Direction placement);
        void RemoveWall(IPlayer player, Vector2 from, Direction placement);
    }
}
