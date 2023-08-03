using System.IO;

namespace Quoridor.ConsoleApp.GameManager
{
    public interface IBoardVisualizerFactory
    {
        IBoardVisualizer CreateVisualizer(
            char playerALabel, char playerBLabel, TextWriter stdOut);
    }
}
