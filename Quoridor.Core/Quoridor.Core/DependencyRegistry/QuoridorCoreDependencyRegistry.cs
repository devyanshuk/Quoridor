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
                Component.For<ICell>().ImplementedBy<Cell>().LifestyleSingleton(),
                Component.For<IWall>().ImplementedBy<Wall>().LifestyleSingleton(),
                Component.For<IBoard>().ImplementedBy<Board>().LifestyleSingleton(),
                Component.For<IGameLogic>().ImplementedBy<GameLogic>().LifestyleSingleton(),
                Component.For<IGameEnvironment>().ImplementedBy<GameEnvironment>().LifestyleSingleton(),

                //Factory
                Component.For<IWallFactory>().AsFactory().LifestyleTransient(),
                Component.For<ICellFactory>().AsFactory().LifestyleTransient(),
            };
        }
    }
}
