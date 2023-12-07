using Quoridor.Core.Game;

namespace Quoridor.ConsoleApp.GameManager
{
    public interface IConsoleGameManagerFactory
    {
        IConsoleGameManager CreateConsoleGameManager(
            ConsoleGameSettings settings, IGameEnvironment gameEnv);
    }
}
