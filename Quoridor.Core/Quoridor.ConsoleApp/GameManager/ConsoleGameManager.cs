using System;
using System.IO;
using System.Text;

using Quoridor.Core.Game;
using Quoridor.Core.Utils;
using Quoridor.Core.Entities;
using Quoridor.Common.Logging;
using Quoridor.Core.Environment;

namespace Quoridor.ConsoleApp.GameManager
{
    public class ConsoleGameManager : IConsoleGameManager
    {
        private readonly int _numWalls;
        private readonly IBoard _board;
        private readonly IBoardVisualizer _boardVisualizer;
        private readonly IGameEnvironment _gameEnvironment;

        private readonly ILogger _log = Logger.InstanceFor<ConsoleGameManager>();

        public ConsoleGameManager(
            int numWalls,
            IBoard board,
            IBoardVisualizer boardVisualizer,
            IGameEnvironment gameEnvironment
        )
        {
            _numWalls = numWalls;
            _board = board;
            _boardVisualizer = boardVisualizer;
            _gameEnvironment = gameEnvironment;
        }

        public void Start()
        {
            var playerStartX = _board.Dimension / 2;
            var playerOne = new Player(_numWalls, new Vector2(playerStartX, 0));
            var playerTwo = new Player(_numWalls, new Vector2(playerStartX, _board.Dimension - 1));

            _log.Info($"{nameof(playerOne)} start pos: '{playerOne.StartPos}'");
            _log.Info($"{nameof(playerTwo)} start pos: '{playerTwo.StartPos}'");

            _gameEnvironment.AddPlayer(playerOne);
            _gameEnvironment.AddPlayer(playerTwo);

            _log.Info($"Starting console game application...");
            while(true)
            {
                _boardVisualizer.DrawBoard();
            }
        }
    }
}
