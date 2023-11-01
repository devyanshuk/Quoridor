using Quoridor.Core.Movement;

namespace Quoridor.ConsoleApp.GameManager.Command
{
    public interface ICommandParser
    {
        Move Parse(string line);
    }
}
