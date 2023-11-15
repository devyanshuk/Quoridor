using Quoridor.Core;

namespace Quoridor.ConsoleApp.GameManager.Command
{
    public interface ICommandParser
    {
        Movement Parse(string line);
    }
}
