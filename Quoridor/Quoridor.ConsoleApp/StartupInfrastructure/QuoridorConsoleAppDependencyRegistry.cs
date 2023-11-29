using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;

using Quoridor.ConsoleApp.GameManager;
using Quoridor.ConsoleApp.Configuration;
using Quoridor.ConsoleApp.GameManager.Command;
using Quoridor.ConsoleApp.GameManager.Visualizer;

namespace Quoridor.ConsoleApp.StartupInfrastructure
{
    public static class QuoridorConsoleAppDependencyRegistry
    {
        public static IRegistration[] GetRegistrations()
        {
            return
            [
                //LifestyleSingleton
                Component.For<ILocalSettings>().ImplementedBy<LocalSettings>().LifestyleSingleton(),
                Component.For<ICommandParser>().ImplementedBy<CommandParser>().LifestyleSingleton(),
                Component.For<IConfigProvider>().ImplementedBy<ConfigProvider>().LifestyleSingleton(),
                Component.For<IBoardVisualizer>().ImplementedBy<BoardVisualizer>().LifestyleSingleton(),
                Component.For<IConsoleGameManager>().ImplementedBy<ConsoleGameManager>().LifestyleSingleton(),

                //Factory
                Component.For<IConsoleGameManagerFactory>().AsFactory().LifestyleSingleton(),
            ];
        }
    }
}
