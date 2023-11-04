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

        public void Initialize()
        {
            StrategyTurn = 0;
            _gameEnvironment.Initialize();
        }

        public void Start()
        {
            _log.Info($"Starting console game application...");

            if (!_settings.Simulate)
                _boardVisualizer.DrawBoard(_settings.OutputDest);

            int simulations = 0;

            while(true)
            { 
                var info = _settings.Strategies[StrategyTurn];

                if (!_settings.Simulate)
                {
                    _settings.OutputDest.WriteLine(@$"Player '{_gameEnvironment.CurrentPlayer}''s Turn. {
                        _gameEnvironment.CurrentPlayer.NumWalls} wall(s) left. Using {info.Strategy.Name} strategy");
                }

                GetAndDoMove(info.Strategy);

                if (!_settings.Simulate)
                    _boardVisualizer.DrawBoard(_settings.OutputDest);

                if (_gameEnvironment.HasFinished)
                {
                    _settings.Strategies[StrategyTurn].GamesWon++;

                    var gameInfo = _settings.Simulate ? simulations.ToString() : String.Empty;
                    _settings.OutputDest.WriteLine(@$"Game {gameInfo} over. Player {_gameEnvironment.CurrentPlayer} : {
                        info.Strategy.Name} won.");

                    if (_settings.Simulate && ++simulations < _settings.NumberOfSimulations)
                        Initialize();

                    else
                        break;
                }

                StrategyTurn = (StrategyTurn + 1) % _gameEnvironment.Players.Count;

                if (!_settings.Simulate && _settings.WaitForInput)
                {
                    _settings.OutputDest.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            if (_settings.Simulate)
            {
                _settings.OutputDest.WriteLine("===Results===");
                foreach (var info in _settings.Strategies)
                {
                    var winRate = (double)(100 * info.GamesWon) / _settings.NumberOfSimulations;
                    _settings.OutputDest.WriteLine(@$"{info.Strategy.Name} won {info.GamesWon}/{
                        _settings.NumberOfSimulations} games. Win rate : {winRate.ToString("0.##")}%");
                }
            }
        }

        public void GetAndDoMove(AIStrategy<Movement, IGameEnvironment, IPlayer> strategy)
        {
            while (true)
            {
                try
                {
                    var result = strategy.BestMove(_gameEnvironment, _gameEnvironment.CurrentPlayer);
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
