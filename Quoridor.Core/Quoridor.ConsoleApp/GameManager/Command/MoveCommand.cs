using Quoridor.Core.Game;
using Quoridor.Common.Logging;

namespace Quoridor.ConsoleApp.GameManager.Command
{
    public sealed class MoveCommand : BaseCommand
    {
        private readonly ILogger _log = Logger.InstanceFor<MoveCommand>();

        public override void Handle(IGameEnvironment gameEnv)
        {
            _log.Info($@"Move command entered. Moving player '{
                gameEnv.CurrentPlayer.Id}' towards '{Dir}'");

            gameEnv.MovePlayer(Dir);
        }
    }
}
