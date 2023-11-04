using System;

using Quoridor.Core.Game;
using Quoridor.Core.Entities;
using Quoridor.AI.Interfaces;
using Quoridor.Common.Logging;
using Quoridor.ConsoleApp.GameManager.Visualizer;
using System.Linq;

namespace Quoridor.ConsoleApp.GameManager
{
    public class ConsoleGameManager : IConsoleGameManager
    {
        private readonly ConsoleGameSettings _settings;
        private readonly IBoardVisualizer _boardVisualizer;
        private readonly IGameEnvironment _gameEnvironment;

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
            _gameEnvironment.Initialize();
        }

        public void Start()
        {
            _log.Info($"Starting console game application...");

            if (!_settings.Simulate)
                _boardVisualizer.DrawBoard(_settings.OutputDest);

            int simulations = 1;

            while(true)
            { 
                var info = _settings.Strategies[_gameEnvironment.Turn];
                var player = _gameEnvironment.CurrentPlayer;

                if (!_settings.Simulate)
                    _settings.OutputDest.WriteLine(@$"Player '{player}''s Turn. {
                        player.NumWalls} wall(s) left. Using {info.Strategy.Name} strategy");

                GetAndDoMove(info.Strategy);

                if (!_settings.Simulate)
                    _boardVisualizer.DrawBoard(_settings.OutputDest);

                if (_gameEnvironment.HasFinished)
                {
                    info.GamesWon++;

                    if (!_settings.Simulate || _settings.Simulate && _settings.Verbose)
                        DisplayGameStats(simulations, info);

                    if (_settings.Simulate && ++simulations <= _settings.NumberOfSimulations)
                        Initialize();

                    else
                        break;
                }
                if (!_settings.Simulate && _settings.WaitForInput)
                {
                    _settings.OutputDest.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            if (_settings.Simulate)
            {
                _settings.OutputDest.WriteLine("===Results===");
                for (int i = 0; i < _gameEnvironment.Players.Count; i++)
                {
                    var info = _settings.Strategies[i];
                    var player = _gameEnvironment.Players[i];
                    var winRate = (double)(100 * info.GamesWon) / _settings.NumberOfSimulations;
                    _settings.OutputDest.WriteLine(@$"Player {player} : {info.Strategy.Name} won {
                        info.GamesWon}/{_settings.NumberOfSimulations} games. Win rate : {winRate.ToString("0.##")}%");
                }
            }
        }

        public void DisplayGameStats(int simulations, StrategyInfo info)
        {
            var gameInfo = _settings.Simulate ? simulations.ToString() : String.Empty;
            var winningPlayer = _gameEnvironment.Players.Single(p => p.Won());
            var losingPlayer = _gameEnvironment.Players.Single(p => !p.Won());
            var losingStrategy = _settings.Strategies[_gameEnvironment.Turn];

            _settings.OutputDest.WriteLine(@$"Game {gameInfo} over. Player {winningPlayer} : {
                info.Strategy.Name} won. Player {losingPlayer} : {losingStrategy.Strategy.Name} lost");
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
