using System;

using Quoridor.Core.Game;
using Quoridor.Core.Entities;
using Quoridor.AI.Interfaces;
using Quoridor.Common.Logging;
using Quoridor.ConsoleApp.GameManager.Visualizer;

namespace Quoridor.ConsoleApp.GameManager
{
    public class ConsoleGameManager : IConsoleGameManager
    {
        private readonly ConsoleGameSettings _settings;
        private readonly IBoardVisualizer _boardVisualizer;
        private readonly IGameEnvironment _gameEnvironment;
        private int StrategyTurn = 0;

        private readonly ILogger _log = Logger.InstanceFor<ConsoleGameManager>();

        public ConsoleGameManager(
            ConsoleGameSettings settings,
            IBoardVisualizer boardVisualizer,
            IGameEnvironment gameEnvironment
        )
        {
            _settings = settings;
            _boardVisualizer = boardVisualizer;
            _gameEnvironment = gameEnvironment;
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

                if (_gameEnvironment.HasFinished)
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
    }
}
