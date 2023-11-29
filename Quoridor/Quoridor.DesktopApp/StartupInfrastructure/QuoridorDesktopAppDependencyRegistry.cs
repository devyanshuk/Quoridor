using Castle.MicroKernel.Registration;

using Quoridor.DesktopApp.Configuration;

namespace Quoridor.DesktopApp.StartupInfrastructure
{
    public class QuoridorDesktopAppDependencyRegistry
    {
        public static IRegistration[] GetRegistrations()
        {
            return
            [
                //LifestyleSingleton
                Component.For<IConfigProvider>().ImplementedBy<ConfigProvider>().LifestyleSingleton(),

                //LifestyleTransient
                Component.For<ILocalSettings>().ImplementedBy<LocalSettings>().LifestyleTransient(),
            ];
        }
    }
}
