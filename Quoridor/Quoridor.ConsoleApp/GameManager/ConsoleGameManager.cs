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

        private float _totalAverageBranchingFactor = 0;

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
                    _settings.OutputDest.WriteLine(@$"Player '{player}''s Turn. {info.SingleGameMoves} moves made. {
                        player.NumWalls} wall(s) left. Using {info.Strategy.Name} strategy");

                GetAndDoMove(info);

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
            if (_settings.BranchingFactor)
            {
                _settings.OutputDest.WriteLine(@$"Average branching factor : {
                    _totalAverageBranchingFactor / _settings.NumberOfSimulations}");
            }
        }

        public void DisplayGameStats(int simulations, StrategyInfo winningStrategy)
        {
            var gameInfo = _settings.Simulate ? simulations.ToString() : String.Empty;
            var winningPlayer = _gameEnvironment.Players.Single(p => p.Won());
            var losingPlayer = _gameEnvironment.Players.Single(p => !p.Won());
            var losingStrategy = _settings.Strategies[_gameEnvironment.Turn];

            _settings.OutputDest.WriteLine(@$"Game {gameInfo} over. Player {
                winningPlayer} : {winningStrategy.Strategy.Name} won in {winningStrategy.SingleGameMoves} moves. Player {
                losingPlayer} : {losingStrategy.Strategy.Name} lost. {losingStrategy.SingleGameMoves} moves made");

            if (_settings.BranchingFactor) {
                var totalGameStates = winningStrategy.SingleGameStates + losingStrategy.SingleGameStates;
                var totalGameMoves = winningStrategy.SingleGameMoves + losingStrategy.SingleGameMoves;
                var branchingFactor = totalGameStates / totalGameMoves;
                _totalAverageBranchingFactor += branchingFactor;

                _settings.OutputDest.WriteLine($"total game states : {totalGameStates}");
                _settings.OutputDest.WriteLine($"total moves : {totalGameMoves}");
                _settings.OutputDest.WriteLine($"average branching factor : {branchingFactor}");
            }

            winningStrategy.SingleGameStates = 0;
            winningStrategy.SingleGameMoves = 0;
            losingStrategy.SingleGameMoves = 0;
            losingStrategy.SingleGameStates = 0;
        }

        public void GetAndDoMove(StrategyInfo info)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var result = info.Strategy.BestMove(_gameEnvironment, _gameEnvironment.CurrentPlayer);
            watch.Stop();

            if (!_settings.BranchingFactor && !_settings.Simulate && _settings.Verbose && !(info.Strategy is HumanAgentConsole))
                _settings.OutputDest.WriteLine($"Time taken to get best move: {watch.ElapsedMilliseconds / 1000.0} seconds");

            Process(result.BestMove, info);
        }


        public void Process<T>(T command, StrategyInfo info) where T : Movement
        {
            _log.Info($"Received '{typeof(T).Name}' command");

            try
            {
                var possibleMoves = _gameEnvironment.GetValidMoves().Count();
                
                _gameEnvironment.Move(command);

                //if move was successful, update the number of moves and game states
                info.SingleGameStates += possibleMoves;
                info.SingleGameMoves++;
            }
            catch(Exception ex)
            {
                _settings.OutputDest.WriteLine(ex.Message);
            }
        }
    }
}
