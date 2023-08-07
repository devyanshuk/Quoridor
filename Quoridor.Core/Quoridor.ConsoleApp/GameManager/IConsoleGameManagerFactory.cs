namespace Quoridor.ConsoleApp.GameManager
{
    public interface IConsoleGameManagerFactory
    {
        IConsoleGameManager CreateManager(ConsoleGameSettings settings);
    }
}
