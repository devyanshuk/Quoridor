using CLAP;
using System.IO;
using System.Linq;
using Castle.Windsor;
using CLAP.Validation;

using Quoridor.Common.Logging;
using Quoridor.ConsoleApp.Utils;
using Quoridor.Core.Environment;
using Quoridor.ConsoleApp.GameManager;

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

            [Description("Player Ids (comma separated chars)")]
            [DefaultValue("A,B")]
            [Aliases("i")]
            string Ids,

            [Description("Number of walls each player has at the beginning")]
            [DefaultValue(8)]
            [Aliases("w")]
            int NumWalls
        )
        {
            _log.Info($@"{nameof(Play)} method called. Params: {nameof(Dimension)}: {Dimension},
            {nameof(Ids)}: {Ids}, {nameof(NumWalls)}: {NumWalls}, {nameof(NumPlayers)}: {NumPlayers}");

            _container.Resolve<IBoard>().SetDimension(Dimension);
            var gameManagerFactory = _container.Resolve<IConsoleGameManagerFactory>();

            var settings = new ConsoleGameSettings
            {
                InputSrc = _stdIn,
                OutputDest = _stdOut,
                NumPlayers = NumPlayers,
                PlayerIds = Ids.Split(",").Select(i => i.Trim()[0]).ToList(),
                NumWalls = NumWalls
            };

            var gameManager = gameManagerFactory.CreateManager(settings);
            gameManager.Start();
        }
    }
}
