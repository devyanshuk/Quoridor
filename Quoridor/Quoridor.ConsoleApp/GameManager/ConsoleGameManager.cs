using System;
using System.Linq;

using Quoridor.Core;
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
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var result = strategy.BestMove(_gameEnvironment, _gameEnvironment.CurrentPlayer);
                watch.Stop();

                if (_settings.Verbose && !(strategy is HumanAgentConsole))
                {
                    _settings.OutputDest.WriteLine($"Time taken to get best move: {watch.ElapsedMilliseconds / 1000.0} seconds");
                    _settings.OutputDest.WriteLine($"A Star algorithm used {_gameEnvironment.ASTAR_COUNT} times");
                    _gameEnvironment.ASTAR_COUNT = 0;
                }

                Process(result.BestMove);
            }
            catch (Exception ex)
            {
                _settings.OutputDest.WriteLine(ex.Message);
            }
        }


        public void Process<T>(T command) where T : Movement
        {
            _log.Info($"Received '{typeof(T).Name}' command");
            _gameEnvironment.Move(command);
        }
    }
}
