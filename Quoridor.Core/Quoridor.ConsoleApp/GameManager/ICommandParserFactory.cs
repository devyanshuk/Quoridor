using System.IO;

namespace Quoridor.ConsoleApp.GameManager
{
    public interface ICommandParserFactory
    {
        ICommandParser CreateParser(TextReader stdIn, TextWriter stdOut);
    }
}
