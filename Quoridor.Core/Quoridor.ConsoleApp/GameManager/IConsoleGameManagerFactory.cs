namespace Quoridor.ConsoleApp.GameManager
{
    public interface IConsoleGameManagerFactory
    {
        IConsoleGameManager CreateManager(
            char playerAId, char playerBId, int numWalls, IBoardVisualizer boardVisualizer);
    }
}
