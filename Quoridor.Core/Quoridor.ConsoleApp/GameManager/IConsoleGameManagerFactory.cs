using System.IO;

namespace Quoridor.ConsoleApp.GameManager
{
    public interface IConsoleGameManagerFactory
    {
        IConsoleGameManager CreateManager(
            char playerALabel, char playerBLabel, int numWalls, TextWriter stdOut);
    }
}
