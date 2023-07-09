using System;
using Castle.Windsor;
using Quoridor.ConsoleApp.DependencyInstaller;
using Quoridor.Core.Environment;
using Quoridor.Core.Game;

namespace Quoridor.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new WindsorContainer().Install(new QuoridorConsoleAppDependencyInstaller());
            container.Resolve<IBoard>().SetDimension(9);

            var game = container.Resolve<IGameLogic>();
            Console.WriteLine("Success");
        }
    }
}
