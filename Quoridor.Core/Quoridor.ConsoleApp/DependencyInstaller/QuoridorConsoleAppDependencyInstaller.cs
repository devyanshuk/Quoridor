using Castle.Windsor;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;

using Quoridor.Core.DependencyRegistry;


namespace Quoridor.ConsoleApp.DependencyInstaller
{
    public class QuoridorConsoleAppDependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<TypedFactoryFacility>();
            container.Register(QuoridorCoreDependencyRegistry.GetRegistrations());
        }
    }
}
