using System;

using Quoridor.Core.Environment;

namespace Quoridor.Core.Game
{
    public class GameLogic : IGameLogic
    {
        private readonly IGameEnvironment _gameEnvironment;

        public GameLogic(IGameEnvironment gameEnvironment)
        {
            _gameEnvironment = gameEnvironment;
        }
    }
}
