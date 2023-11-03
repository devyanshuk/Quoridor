using System.IO;

namespace Quoridor.ConsoleApp.GameManager.Visualizer
{
    public interface IBoardVisualizer
    {
        void DrawBoard(TextWriter dest);
    }
}
