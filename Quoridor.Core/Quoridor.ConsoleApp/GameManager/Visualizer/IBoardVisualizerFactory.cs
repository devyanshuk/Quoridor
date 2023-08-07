using System.IO;

namespace Quoridor.ConsoleApp.GameManager.Visualizer
{
    public interface IBoardVisualizerFactory
    {
        IBoardVisualizer CreateVisualizer(TextWriter stdOut);
    }
}
