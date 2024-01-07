using CLAP;
using System.IO;
using Castle.Windsor;
using CLAP.Validation;
using System.Globalization;
using System.Collections.Generic;

using Quoridor.AI;
using Quoridor.Core;
using Quoridor.AI.MCTS;
using Quoridor.AI.Random;
using Quoridor.Core.Game;
using Quoridor.AI.Interfaces;
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

        [Verb]
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
            [DefaultValue(false)]
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
            [DefaultValue("1.41")]
            [Aliases("exploration")]
            string C,

            [Description("Simulation limit for MCTS")]
            [DefaultValue(1000)]
            [Aliases("mctsims")]
            int MctsSim,

            [Description("Agent to perform MCTS simulation")]
            [DefaultValue("semirandom")]
            [StrategyValidation]
            [Aliases("agent")]
            string MctsAgent,

            [Description("Compute the average branching factor for a specified dimension")]
            [DefaultValue(false)]
            [Aliases("b")]
            bool BranchingFactor
            )
        {
            // for large tree, logs might be very big, so we disable it.
            Logger.Disable = true;

            double c = double.Parse(C, CultureInfo.InvariantCulture);

            if (BranchingFactor) Simulate = true;

            _container.Resolve<IBoard>().SetDimension(Dimension);

            var settings = new ConsoleGameSettings
            {
                Verbose = Verbose,
                Simulate = Simulate,
                BranchingFactor = BranchingFactor,
                NumberOfSimulations = NumSimulate,
                WaitForInput = WaitForInput,
                OutputDest = _stdOut,
                Strategies = new List<StrategyInfo>()
            };

            //add selected ai/human strategy
            var mctsAgent = EnumHelper.ParseEnum<AITypes>(MctsAgent.ToUpper());
            settings.Strategies.Add(
                GetStrategyInfo(EnumHelper.ParseEnum<AITypes>(Strategy1.ToUpper()), mctsAgent, Depth, Seed, c, MctsSim));
            settings.Strategies.Add(
                GetStrategyInfo(EnumHelper.ParseEnum<AITypes>(Strategy2.ToUpper()), mctsAgent, Depth, Seed, c, MctsSim));

            var gameEnv = _container
                .Resolve<IGameFactory>()
                .CreateGameEnvironment(2, NumWalls);

            var manager = _gameManagerFactory.CreateConsoleGameManager(settings, gameEnv);
            manager.Start();
        }

        private StrategyInfo GetStrategyInfo(AITypes aiType, AITypes sim, int depth, int seed, double c, int mctSim)
        {
            switch (aiType)
            {
                case AITypes.MCTS:
                    {
                        var selectionStrategy = new UCT<Movement, IPlayer, IGameEnvironment>(c);
                        //if minimax is selected, use depth of 1. that way, it'll use a greedy agent that's relatively faster
                        var moveStrategy = GetStrategy(sim, 1, seed);
                        return new StrategyInfo {
                            Strategy = new MonteCarloTreeSearch<Movement, IGameEnvironment, IPlayer>(
                                mctSim, selectionStrategy, moveStrategy)};
                    }
                default:
                    return new StrategyInfo { Strategy = GetStrategy(aiType, depth, seed) };
            }
        }

        private IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy(AITypes aitype, int depth, int seed)
        {
            return aitype switch
            {
                AITypes.ASTAR => new AStar<Movement, IGameEnvironment, IPlayer>(),
                AITypes.RANDOM => new RandomStrategy<Movement, IGameEnvironment, IPlayer>(seed),
                AITypes.SEMIRANDOM => new SemiRandomStrategy<IGameEnvironment, Movement, IPlayer>(
                    seed, new AStar<Movement, IGameEnvironment, IPlayer>()),
                AITypes.HUMAN => new HumanAgentConsole(_stdIn, _stdOut, _container.Resolve<ICommandParser>()),
                AITypes.MINIMAX => new Minimax<IPlayer, Movement, IGameEnvironment>(depth),
                AITypes.MINIMAXAB => new MinimaxABPruning<IPlayer, Movement, IGameEnvironment>(depth),
                AITypes.PARALLELMINIMAXAB => new ParallelMinimaxABPruning<IPlayer, Movement, IGameEnvironment>(depth),
                _ => new Minimax<IPlayer, Movement, IGameEnvironment>(depth)
            };

        }
    }
}
