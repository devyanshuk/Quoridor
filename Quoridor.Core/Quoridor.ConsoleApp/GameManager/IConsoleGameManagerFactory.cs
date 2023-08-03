namespace Quoridor.ConsoleApp.GameManager
{
    public interface IConsoleGameManagerFactory
    {
        IConsoleGameManager CreateManager(
            int numWalls, IBoardVisualizer boardVisualizer);
    }
}
