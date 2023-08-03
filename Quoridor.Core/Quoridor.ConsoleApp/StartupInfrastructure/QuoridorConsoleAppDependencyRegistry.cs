using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Quoridor.ConsoleApp.GameManager;
using Quoridor.ConsoleApp.Configuration;

namespace Quoridor.ConsoleApp.StartupInfrastructure
{
    public static class QuoridorConsoleAppDependencyRegistry
    {
        public static IRegistration[] GetRegistrations()
        {
            return new IRegistration[]
            {
                //LifestyleSingleton
                Component.For<IConsoleGameManager>().ImplementedBy<ConsoleGameManager>().LifestyleSingleton(),
                Component.For<IConfigProvider>().ImplementedBy<ConfigProvider>().LifestyleSingleton(),
                Component.For<IBoardVisualizer>().ImplementedBy<BoardVisualizer>().LifestyleSingleton(),

                //Factory
                Component.For<IConsoleGameManagerFactory>().AsFactory().LifestyleSingleton(),
                Component.For<IBoardVisualizerFactory>().AsFactory().LifestyleSingleton(),
            };
        }
    }
}
