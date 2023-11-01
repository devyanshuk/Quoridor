﻿using System;

using Quoridor.Core.Game;
using Quoridor.Core.Utils;
using Quoridor.Core.Movement;
using Quoridor.Core.Entities;
using Quoridor.Common.Logging;
using Quoridor.Core.Environment;
using Quoridor.AI.AStarAlgorithm;
using Quoridor.ConsoleApp.GameManager.Command;
using Quoridor.ConsoleApp.GameManager.Visualizer;

namespace Quoridor.ConsoleApp.GameManager
{
    public class ConsoleGameManager : IConsoleGameManager
    {
        private readonly ConsoleGameSettings _settings;
        private readonly IBoard _board;
        private readonly IBoardVisualizer _boardVisualizer;
        private readonly IGameEnvironment _gameEnvironment;
        private readonly ICommandParser _commandParser;

        private readonly ILogger _log = Logger.InstanceFor<ConsoleGameManager>();

        public ConsoleGameManager(
            ConsoleGameSettings settings,
            IBoard board,
            IBoardVisualizer boardVisualizer,
            IGameEnvironment gameEnvironment,
            ICommandParser commandParser
        )
        {
            _settings = settings;
            _board = board;
            _boardVisualizer = boardVisualizer;
            _gameEnvironment = gameEnvironment;
            _commandParser = commandParser;

            InitAndAddPlayers();
        }

        public void Start()
        {
            _log.Info($"Starting console game application...");
            while(true)
            { 
                _boardVisualizer.DrawBoard(_settings.OutputDest);

                var player = _gameEnvironment.CurrentPlayer;
                _settings.OutputDest.WriteLine($"Player '{player.Id}''s Turn. {player.NumWalls} wall(s) left");

                ParseAndProcessCommand();
                _gameEnvironment.ChangeTurn();
            }
        }

        public void ParseAndProcessCommand()
        {
            while (true)
            {
                var line = _settings.InputSrc.ReadLine();
                try
                {
                    var command = _commandParser.Parse(line);
                    Process(command);
                    break;
                }
                catch (Exception ex)
                {
                    _settings.OutputDest.WriteLine(ex.Message);
                }
            }
        }

        public void Process<T>(T command) where T : Move
        {
            _log.Info($"Received '{typeof(T).Name}' command");
            _gameEnvironment.Move(command);
        }

        private void InitAndAddPlayers()
        {
            var startXs = new int[4] { _board.Dimension / 2, _board.Dimension / 2, 0, _board.Dimension - 1 };
            var startYs = new int[4] { 0, _board.Dimension - 1, _board.Dimension / 2, _board.Dimension / 2 };
            var goalConditions = new IsGoal<Vector2>[] {
                (pos) => pos.Y == _board.Dimension - 1,
                (pos) => pos.Y == 0,
                (pos) => pos.X == 0,
                (pos) => pos.X == _board.Dimension - 1
            };
            var heuristics = new H_n<Vector2>[]
            {
                (pos) => Math.Abs(_board.Dimension - 1 - pos.Y),
                (pos) => pos.Y,
                (pos) => pos.X,
                (pos) => Math.Abs(_board.Dimension - 1 - pos.Y)
            };

            var currIdAscii = 65;

            for (int i = 0; i < _settings.NumPlayers; i++)
            {
                var startX = startXs[i];
                var startY = startYs[i];
                var playerId = (char)currIdAscii++;

                var startPos = new Vector2(startX, startY);
                var player = new Player(playerId, _settings.NumWalls, startPos)
                {
                    ManhattanHeuristicFn = heuristics[i],
                    IsGoalCell = goalConditions[i]
                };

                _gameEnvironment.AddPlayer(player);

                _log.Info($"Successfully added player '{playerId}'. Start pos: '{startPos}'");
            }
        }
    }
}
