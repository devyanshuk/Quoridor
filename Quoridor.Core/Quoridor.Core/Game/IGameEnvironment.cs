using System.Collections.Generic;

using Quoridor.Core.Utils;
using Quoridor.AI.Interfaces;
using Quoridor.Core.Entities;

namespace Quoridor.Core.Game
{
    public interface IGameEnvironment :
        IGame<IPlayer, Movement>,
        INeighbors<Vector2>,
        INeighbors<Movement>
    {
        List<IPlayer> Players { get; }

        void Initialize();

        void AddPlayer(IPlayer player);
        void MovePlayer(Direction dir);
        void ChangeTurn();

        void AddWall(Vector2 from, Direction placement);
        void RemoveWall(Vector2 from, Direction placement);
    }
}
