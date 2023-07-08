using System;

namespace Quoridor.Core.Environment
{
    public class GameEnvironment : IGameEnvironment
    {
        private readonly IBoard _board;

        public GameEnvironment(IBoard board)
        {
            _board = board;
        }
    }
}
