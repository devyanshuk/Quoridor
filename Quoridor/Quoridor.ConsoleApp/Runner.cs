using CLAP;
using System;
using System.IO;
using Castle.Windsor;
using CLAP.Validation;
using System.Collections.Generic;

using Quoridor.AI;
using Quoridor.AI.Random;
using Quoridor.Core.Game;
using Quoridor.AI.Interfaces;
using Quoridor.Core.Entities;
using Quoridor.Common.Logging;
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
        }

        [Verb(IsDefault=true)]
        public void Play(

            [Description("Game Board Dimension")]
            [DefaultValue(9)]
            [Aliases("d")]
            int Dimension,

            [Description("Number of Players")]
            [LessOrEqualTo(4), MoreOrEqualTo(0)]
            [DefaultValue(2)]
            [Aliases("n")]
            int NumPlayers,

            [Description("Number of walls each player has at the beginning")]
            [DefaultValue(8)]
            [Aliases("w")]
            int NumWalls
        )
        {
            _log.Info($@"{nameof(Play)} method called. Params: {nameof(Dimension)}: {Dimension},
            {nameof(NumWalls)}: {NumWalls}, {nameof(NumPlayers)}: {NumPlayers}");

            _container.Resolve<IBoard>().SetDimension(Dimension);
            var gameManagerFactory = _container.Resolve<IConsoleGameManagerFactory>();
            var commandParser = _container.Resolve<ICommandParser>();

            var settings = new ConsoleGameSettings { OutputDest = _stdOut };
            settings.Strategies = new List<AIStrategy<Movement, IGameEnvironment, IPlayer>>();
            for (int i = 0; i < NumPlayers; i++)
                settings.Strategies.Add(new HumanAgentConsole(_stdIn, commandParser));

            var gameEnv = _container
                .Resolve<IGameFactory>()
                .CreateGameEnvironment(NumPlayers, NumWalls);

            var gameManager = gameManagerFactory.CreateManager(settings, gameEnv);
            gameManager.Start();
        }

        [Verb]
        public void PlayVsAI(
            [Description("Game Board Dimension")]
            [DefaultValue(9)]
            [Aliases("d")]
            int Dimension,

            [Description("Number of walls each player has at the beginning")]
            [DefaultValue(8)]
            [Aliases("w")]
            int NumWalls,

            [Description("AI to play against")]
            [DefaultValue("AStar")]
            [Aliases("a")]
            string AI,

            [Description("Depth of the search tree")]
            [DefaultValue(1)]
            [Aliases("de")]
            int Depth,

            [Description("Random seed")]
            [DefaultValue(20)]
            [Aliases("s")]
            int Seed
            )
        {
            // for large tree, logs might be very big, so we disable it.
            Logger.Disable = true;

            _container.Resolve<IBoard>().SetDimension(Dimension);
            var gameManagerFactory = _container.Resolve<IConsoleGameManagerFactory>();
            var commandParser = _container.Resolve<ICommandParser>();

            var settings = new ConsoleGameSettings
            {
                OutputDest = _stdOut,
                Strategies = new List<AIStrategy<Movement, IGameEnvironment, IPlayer>>()
            };
            //add selected ai
            var aiType = ParseEnum<AITypes>(AI);
            settings.Strategies.Add(GetStrategy(aiType, Depth, Seed));
            //add human player
            settings.Strategies.Add(new HumanAgentConsole(_stdIn, commandParser));

            var gameEnv = _container
                .Resolve<IGameFactory>()
                .CreateGameEnvironment(2, NumWalls);
            var gameManager = gameManagerFactory.CreateManager(settings, gameEnv);
            gameManager.Start();
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(enumType: typeof(T), value: value, ignoreCase: true);
        }

        private AIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy(AITypes aiType, int depth, int seed)
        {
            switch (aiType)
            {
                case AITypes.AStar:
                    return new AStar<Movement, IGameEnvironment, IPlayer>();
                case AITypes.Random:
                    return new RandomStrategy<Movement, IGameEnvironment, IPlayer>(seed);
                default:
                    return new Minimax<IPlayer, Movement, IGameEnvironment>(depth);
            }
        }
    }
}
