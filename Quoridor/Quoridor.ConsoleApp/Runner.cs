﻿using CLAP;
using System.IO;
using Castle.Windsor;
using CLAP.Validation;
using System.Collections.Generic;

using Quoridor.AI;
using Quoridor.Core;
using Quoridor.AI.MCTS;
using Quoridor.AI.Random;
using Quoridor.Core.Game;
using Quoridor.Core.Entities;
using Quoridor.Common.Logging;
using Quoridor.Common.Helpers;
using Quoridor.ConsoleApp.Utils;
using Quoridor.Core.Environment;
using Quoridor.AI.AStarAlgorithm;
using Quoridor.AI.MinimaxAlgorithm;
using Quoridor.ConsoleApp.GameManager;
using Quoridor.ConsoleApp.GameManager.Command;

namespace Quoridor.ConsoleApp
{
    public class Runner : BaseRunner
    {
        private readonly IWindsorContainer _container;
        private readonly IConsoleGameManagerFactory _gameManagerFactory;
        private readonly ILogger _log = Logger.InstanceFor<Runner>();

        private readonly TextReader _stdIn;

        public Runner(
            IWindsorContainer container,
            TextReader stdIn,
            TextWriter stdOut,
            TextWriter stdErr)
            : base(stdOut, stdErr)
        {
            _stdIn = stdIn;
            _container = container;
            _gameManagerFactory = _container.Resolve<IConsoleGameManagerFactory>();
        }

        [Verb(IsDefault = true)]
        public void Play(
            [Description("Game Board Dimension")]
            [DefaultValue(5), MoreOrEqualTo(3)]
            [Aliases("dim")]
            int Dimension,

            [Description("Number of walls each player has at the beginning")]
            [DefaultValue(8), MoreOrEqualTo(0)]
            [Aliases("walls")]
            int NumWalls,

            [Description("First strategy")]
            [DefaultValue("Minimax")]
            [StrategyValidation]
            [Aliases("s1")]
            string Strategy1,

            [Description("Second strategy")]
            [DefaultValue("Human")]
            [StrategyValidation]
            [Aliases("s2")]
            string Strategy2,

            [Description("Enable viewer to press any key before the other agent makes move")]
            [DefaultValue(false)]
            [Aliases("wait")]
            bool WaitForInput,

            [Description("Simulate games")]
            [DefaultValue(false)]
            [Aliases("sim")]
            bool Simulate,

            [Description("Display all results of the simulation")]
            [DefaultValue(true)]
            [Aliases("v")]
            bool Verbose,

            [Description("Number of games to simulate")]
            [DefaultValue(100), MoreOrEqualTo(0)]
            [Aliases("numsim")]
            int NumSimulate,

            [Description("Maximum depth of the search tree")]
            [DefaultValue(2), MoreOrEqualTo(0)]
            [Aliases("d")]
            int Depth,

            [Description("Seed for the random number generator")]
            [DefaultValue(20)]
            [Aliases("s")]
            int Seed,

            [Description("Exploration parameter for UCT")]
            [DefaultValue(1.41)]
            [Aliases("exploration")]
            double C,

            [Description("Simulation limit for MCTS")]
            [DefaultValue(1000)]
            [Aliases("mctsims")]
            int MctsSim
            )
        {
            // for large tree, logs might be very big, so we disable it.
            Logger.Disable = true;

            _container.Resolve<IBoard>().SetDimension(Dimension);

            var settings = new ConsoleGameSettings
            {
                Verbose = Verbose,
                Simulate = Simulate,
                NumberOfSimulations = NumSimulate,
                WaitForInput = WaitForInput,
                OutputDest = _stdOut,
                Strategies = new List<StrategyInfo>()
            };

            //add selected ai/human strategy
            settings.Strategies.Add(GetStrategy(EnumHelper.ParseEnum<AITypes>(Strategy1), Depth, Seed, C, MctsSim));
            settings.Strategies.Add(GetStrategy(EnumHelper.ParseEnum<AITypes>(Strategy2), Depth, Seed, C, MctsSim));

            var gameEnv = _container
                .Resolve<IGameFactory>()
                .CreateGameEnvironment(2, NumWalls);

            _gameManagerFactory.CreateManager(settings, gameEnv).Start();
        }

        private StrategyInfo GetStrategy(AITypes aiType, int depth, int seed, double c, int mctSim)
        {
            switch (aiType)
            {
                case AITypes.AStar:
                    return new StrategyInfo { Strategy = new AStar<Movement, IGameEnvironment, IPlayer>() };
                case AITypes.Random:
                    return new StrategyInfo { Strategy = new RandomStrategy<Movement, IGameEnvironment, IPlayer>(seed) }; 
                case AITypes.Human:
                    return new StrategyInfo { Strategy = new HumanAgentConsole(_stdIn, _stdOut, _container.Resolve<ICommandParser>()) };
                case AITypes.MinimaxAB:
                    return new StrategyInfo { Strategy = new MinimaxABPruning<IPlayer, Movement, IGameEnvironment>(depth) };
                case AITypes.ParallelMinimaxAB:
                    return new StrategyInfo { Strategy = new ParallelMinimaxABPruning<IPlayer, Movement, IGameEnvironment>(depth) };
                case AITypes.MonteCarlo:
                    {
                        var selectionStrategy = new UCT<Movement, IPlayer, IGameEnvironment>(c);
                        var moveStrategy = new RandomStrategy<Movement, IGameEnvironment, IPlayer>(seed);
                        return new StrategyInfo {
                            Strategy = new MonteCarloTreeSearch<Movement, IGameEnvironment, IPlayer>(mctSim, selectionStrategy, moveStrategy)};
                    }
                default:
                    return new StrategyInfo { Strategy = new Minimax<IPlayer, Movement, IGameEnvironment>(depth) };
            }
        }
    }
}
