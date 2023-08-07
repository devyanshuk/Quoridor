using CLAP;
using System;
using Castle.Windsor;

using Quoridor.Common.Logging;
using Quoridor.ConsoleApp.StartupInfrastructure;

namespace Quoridor.ConsoleApp
{
    
    public class Program
    {
        private static readonly ILogger _log = Logger.InstanceFor<Program>();

        public static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            var container = new WindsorContainer().Install(new QuoridorConsoleAppDependencyInstaller());
            _log.Info($"Successfully installed DI container. Starting {nameof(ConsoleApp)}...");

            var p = new Parser<Runner>();
            p.Run(args, new Runner(container, Console.In, Console.Out, Console.Error));

            _log.Info($"Game finished.");
        }
    }
}
