using CLAP;
using System.IO;
using Castle.Windsor;

using Quoridor.Core.Environment;
using Quoridor.Common.Logging;
using Quoridor.ConsoleApp.Utils;
using Quoridor.ConsoleApp.GameManager;

namespace Quoridor.ConsoleApp
{
    public class Runner : BaseRunner
    {
        private readonly IWindsorContainer _container;
        private readonly ILogger _log = Logger.InstanceFor<Runner>();

        public Runner(
            IWindsorContainer container,
            TextWriter stdOut,
            TextWriter stdErr)
            : base(stdOut, stdErr)
        {
            _container = container;
        }

        [Verb(IsDefault=true)]
        public void Play(

            [Description("Game Board Dimension")]
            [DefaultValue(9)]
            [Aliases("d")]
            int Dimension,

            [Description("Player 1 Identifier")]
            [DefaultValue('A')]
            [Aliases("f")]
            char PlayerOneId,

            [Description("Player 2 Identifier")]
            [DefaultValue('B')]
            [Aliases("s")]
            char PlayerTwoId,

            [Description("Number of walls a player has at the beginning")]
            [DefaultValue(8)]
            [Aliases("w")]
            int NumWalls
        )
        {
            _log.Info($@"{nameof(Play)} method called. Params: {nameof(Dimension)}: {Dimension},
            {nameof(PlayerOneId)} : '{PlayerOneId}', {nameof(PlayerTwoId)}: '{PlayerTwoId}', {nameof(NumWalls)}: {NumWalls}");

            _container.Resolve<IBoard>().SetDimension(Dimension);

            var gameManagerFactory = _container.Resolve<IConsoleGameManagerFactory>();
            var boardVisualizerFactory = _container.Resolve<IBoardVisualizerFactory>();
            var boardVisualizer = boardVisualizerFactory.CreateVisualizer(PlayerOneId, PlayerTwoId, _stdOut);
            var gameManager = gameManagerFactory.CreateManager(NumWalls, boardVisualizer);
            gameManager.Start();
        }
    }
}
