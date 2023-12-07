using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;

using Quoridor.Core.Game;
using Quoridor.Core.Environment;

namespace Quoridor.Core.DependencyRegistry
{
    public static class QuoridorCoreDependencyRegistry
    {
        public static IRegistration[] GetRegistrations()
        {
            return
            [
                //LifestyleSingleton
                Component.For<IBoard>().ImplementedBy<Board>().LifestyleSingleton(),
                Component.For<IGameEnvironment>().ImplementedBy<GameEnvironment>().LifestyleSingleton(),

                //LifestyleTransient
                Component.For<IWall>().ImplementedBy<Wall>().LifestyleTransient(),

                //Factory
                Component.For<IGameFactory>().AsFactory().LifestyleSingleton(),
            ];
        }
    }
}
