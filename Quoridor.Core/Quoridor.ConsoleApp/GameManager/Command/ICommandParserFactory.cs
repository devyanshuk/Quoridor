using System.IO;

namespace Quoridor.ConsoleApp.GameManager.Command
{
    public interface ICommandParserFactory
    {
        ICommandParser CreateParser(TextReader stdIn, TextWriter stdOut);
    }
}
