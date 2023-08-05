using System.IO;

namespace Quoridor.ConsoleApp.GameManager
{
    public interface IBoardVisualizerFactory
    {
        IBoardVisualizer CreateVisualizer(TextWriter stdOut);
    }
}
