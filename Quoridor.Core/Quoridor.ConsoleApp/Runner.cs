using CLAP;
using System;
using System.IO;
using Castle.Windsor;

using Quoridor.Core.Environment;
using Quoridor.Core.Game;
using Quoridor.ConsoleApp.Utils;
using Quoridor.Common.Logging;

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

        [Verb]
        public void Play(

            [Description("Game Board Dimension"), DefaultValue(9)]
            [Aliases("d")]
            int Dimension
        )
        {
            _container.Resolve<IBoard>().SetDimension(Dimension);

            var game = _container.Resolve<IGameLogic>();
        }
    }
}
