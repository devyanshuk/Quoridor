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
        private readonly ICommandParser _commandParser;

        private readonly ILogger _log = Logger.InstanceFor<ConsoleGameManager>();

        public ConsoleGameManager(
            char playerAId,
            char playerBId,
            int numWalls,
            IBoard board,
            IBoardVisualizer boardVisualizer,
            IGameEnvironment gameEnvironment,
            ICommandParser commandParser
        )
        {
            _numWalls = numWalls;
            _board = board;
            _boardVisualizer = boardVisualizer;
            _gameEnvironment = gameEnvironment;
            _commandParser = commandParser;

            InitAndAddPlayers(playerAId, playerBId);
        }

        public void Start()
        {
            _log.Info($"Starting console game application...");
            while(true)
            {
                _boardVisualizer.DrawBoard();
                _commandParser.ParseAndProcess();
            }
        }

        private void InitAndAddPlayers(char playerAId, char playerBId)
        {
            var playerStartX = _board.Dimension / 2;
            var playerOne = new Player(playerAId, _numWalls, new Vector2(playerStartX, 0));
            var playerTwo = new Player(playerBId, _numWalls, new Vector2(playerStartX, _board.Dimension - 1));

            _log.Info($"{nameof(playerOne)} '{playerAId}' start pos: '{playerOne.StartPos}'");
            _log.Info($"{nameof(playerTwo)} '{playerBId}' start pos: '{playerTwo.StartPos}'");

            _gameEnvironment.AddPlayer(playerOne);
            _gameEnvironment.AddPlayer(playerTwo);
        }
    }
}
