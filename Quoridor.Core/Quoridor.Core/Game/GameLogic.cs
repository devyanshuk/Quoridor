using System;

using Quoridor.Core.Environment;
using Quoridor.Core.Utils;

namespace Quoridor.Core.Game
{
    public class GameLogic : IGameLogic
    {
        private readonly IGameEnvironment _gameEnvironment;

        public GameLogic(
            IGameEnvironment gameEnvironment
            )
        {
            _gameEnvironment = gameEnvironment;
        }


        

    }
}
