using System.IO;

namespace Quoridor.ConsoleApp.GameManager.Command
{
    public interface ICommandParser
    {
        BaseCommand Parse(string line);
    }
}
