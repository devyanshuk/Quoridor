using System;
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
            return new IRegistration[]
            {
                //LifestyleSingleton
                Component.For<IBoard>().ImplementedBy<Board>().LifestyleSingleton(),
                Component.For<IGameLogic>().ImplementedBy<GameLogic>().LifestyleSingleton(),
                Component.For<IGameEnvironment>().ImplementedBy<GameEnvironment>().LifestyleSingleton(),

                //Factory
                Component.For<IBoardFactory>().AsFactory(),
                Component.For<IWallFactory>().AsFactory(),
            };
        }
    }
}
