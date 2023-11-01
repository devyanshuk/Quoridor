using System.Collections.Generic;

using Quoridor.AI.Interfaces;

using Quoridor.Core.Utils;
using Quoridor.Core.Entities;
using Quoridor.Core.Movement;

namespace Quoridor.Core.Game
{
    public interface IGameEnvironment :
        IGame<IPlayer, Move>,
        INeighbors<Vector2>
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
