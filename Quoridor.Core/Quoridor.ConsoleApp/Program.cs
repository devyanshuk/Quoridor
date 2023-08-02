using CLAP;
using System;
using Castle.Windsor;

using Quoridor.Common.Logging;
using Quoridor.ConsoleApp.DependencyInstaller;

namespace Quoridor.ConsoleApp
{
    
    public class Program
    {
        private static readonly ILogger _log = Logger.InstanceFor<Program>();

        public static void Main(string[] args)
        {
            var container = new WindsorContainer().Install(new QuoridorConsoleAppDependencyInstaller());
            try
            {
                _log.Info($"Successfully installed DI container. Starting {nameof(ConsoleApp)}...");

                var p = new Parser<Runner>();
                p.Run(args, new Runner(container, Console.Out, Console.Error));
            } catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("HERE");
            //Console.ReadKey();
        }
    }
}
