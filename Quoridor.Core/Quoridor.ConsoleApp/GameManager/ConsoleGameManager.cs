using System;

using Quoridor.Core.Game;
using Quoridor.Core.Utils;
using Quoridor.Core.Entities;
using Quoridor.AI.Interfaces;
using Quoridor.Common.Logging;
using Quoridor.Core.Environment;
using Quoridor.AI.AStarAlgorithm;
using Quoridor.ConsoleApp.GameManager.Visualizer;

namespace Quoridor.ConsoleApp.GameManager
{
    public class ConsoleGameManager : IConsoleGameManager
    {
        private readonly ConsoleGameSettings _settings;
        private readonly IBoard _board;
        private readonly IBoardVisualizer _boardVisualizer;
        private readonly IGameEnvironment _gameEnvironment;
        private int StrategyTurn = 0;

        private readonly ILogger _log = Logger.InstanceFor<ConsoleGameManager>();

        public ConsoleGameManager(
            ConsoleGameSettings settings,
            IBoard board,
            IBoardVisualizer boardVisualizer,
            IGameEnvironment gameEnvironment
        )
        {
            _settings = settings;
            _board = board;
            _boardVisualizer = boardVisualizer;
            _gameEnvironment = gameEnvironment;

            InitAndAddPlayers();
        }

        public void Start()
        {
            _log.Info($"Starting console game application...");
            _boardVisualizer.DrawBoard(_settings.OutputDest);

            while(true)
            { 
                var strategy = _settings.Strategies[StrategyTurn];

                _settings.OutputDest.WriteLine(@$"Player '{_gameEnvironment.CurrentPlayer}''s Turn. {
                    _gameEnvironment.CurrentPlayer} wall(s) left. Using {strategy.Name} strategy");

                GetAndDoMove(strategy);

                _boardVisualizer.DrawBoard(_settings.OutputDest);

                if (_gameEnvironment.IsTerminal)
                    break;

                StrategyTurn = (StrategyTurn + 1) % _settings.NumPlayers;
            }
            _settings.OutputDest.WriteLine(@$"Game over. Player {_gameEnvironment.CurrentPlayer} : {
                _settings.Strategies[StrategyTurn].Name} won.");
        }

        public void GetAndDoMove(AIStrategy<Movement, IGameEnvironment, IPlayer> strategy)
        {
            while (true)
            {
                try
                {
                    var result = strategy.BestMove(_gameEnvironment, _gameEnvironment.CurrentPlayer);
                    Console.WriteLine($"{strategy.Name} made {result}");
                    Process(result.BestMove);
                    break;
                }
                catch(Exception ex)
                {
                    _settings.OutputDest.WriteLine(ex.Message);
                }
            }
        }


        public void Process<T>(T command) where T : Movement
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
                    IsGoalMove = goalConditions[i]
                };

                _gameEnvironment.AddPlayer(player);

                _log.Info($"Successfully added player '{playerId}'. Start pos: '{startPos}'");
            }
        }
    }
}
