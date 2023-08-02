using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;

using Quoridor.ConsoleApp.GameManager;

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

                //Factory
                Component.For<IConsoleGameManagerFactory>().AsFactory().LifestyleSingleton(),
            };
        }
    }
}
