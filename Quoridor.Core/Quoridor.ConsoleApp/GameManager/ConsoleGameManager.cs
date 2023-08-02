using System;
using System.IO;

using Quoridor.Core.Game;
using Quoridor.Common.Logging;
using Quoridor.Core.Environment;
using Quoridor.Core.Entities;
using Quoridor.Core.Utils;
using System.Text;
using Quoridor.ConsoleApp.Configuration;

namespace Quoridor.ConsoleApp.GameManager
{
    public class ConsoleGameManager : IConsoleGameManager
    {
        private readonly IBoard _board;
        private readonly IGameEnvironment _gameEnvironment;
        private readonly IConfigProvider _configProvider;

        private readonly ILogger _log = Logger.InstanceFor<ConsoleGameManager>();

        private readonly char _playerALabel;
        private readonly char _playerBLabel;
        private readonly int _numWalls;
        private readonly TextWriter _stdOut;

        public ConsoleGameManager(
            char playerALabel,
            char playerBLabel,
            int numWalls,
            TextWriter stdOut,
            IBoard board,
            IGameEnvironment gameEnvironment,
            IConfigProvider configProvider
        )
        {
            _playerALabel = playerALabel;
            _playerBLabel = playerBLabel;
            _numWalls = numWalls;
            _stdOut = stdOut;
            _board = board;
            _gameEnvironment = gameEnvironment;
            _configProvider = configProvider;
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

            }
        }

        public void DrawBoard()
        {
            var sb = new StringBuilder();
        }

    }
}
