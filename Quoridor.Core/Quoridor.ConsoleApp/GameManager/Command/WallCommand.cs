using Quoridor.Core.Game;
using Quoridor.Core.Utils;
using Quoridor.Common.Logging;

namespace Quoridor.ConsoleApp.GameManager.Command
{
    public sealed class WallCommand : BaseCommand
    {
        public Vector2 Pos { get; set; }

        private readonly ILogger _log = Logger.InstanceFor<WallCommand>();

        public override void Handle(IGameEnvironment gameEnv)
        {
            gameEnv.AddWall(Pos, Dir);
            _log.Info($"Successfully added wall '{Pos}': '{Dir}'");
        }
    }
}
