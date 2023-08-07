using Quoridor.Core.Game;
using Quoridor.Core.Utils;

namespace Quoridor.ConsoleApp.GameManager.Command
{
    public abstract class BaseCommand
    {
        public Direction Dir { get; set; }

        public abstract void Handle(IGameEnvironment gameEnv);
    }
}
