using Quoridor.Core.Game;

namespace Quoridor.ConsoleApp.GameManager
{
    public interface IConsoleGameManagerFactory
    {
        IConsoleGameManager CreateManager(
            ConsoleGameSettings settings, IGameEnvironment gameEnvironment);
    }
}
