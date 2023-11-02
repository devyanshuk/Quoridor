using System.IO;

using Quoridor.Core.Game;
using Quoridor.AI.Interfaces;
using Quoridor.Core.Entities;
using Quoridor.ConsoleApp.GameManager.Command;

namespace Quoridor.ConsoleApp.GameManager
{
    public class HumanAgentConsole : AIAgent<Movement, IGameEnvironment, IPlayer>
    {
        private readonly TextReader _inputSrc;
        private readonly ICommandParser _commandParser;

        public HumanAgentConsole(
            TextReader inputSrc,
            ICommandParser commandParser)
        {
            _inputSrc = inputSrc;
            _commandParser = commandParser;
        }

        public override string Name => nameof(HumanAgentConsole);

        public override Movement BestMove(IGameEnvironment gameView, IPlayer player)
        {
            var line = _inputSrc.ReadLine();
            var move = _commandParser.Parse(line);
            return move;
        }
    }
}
