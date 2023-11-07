using System;
using System.IO;

using Quoridor.Core.Game;
using Quoridor.AI.Interfaces;
using Quoridor.Core.Entities;
using Quoridor.ConsoleApp.GameManager.Command;

namespace Quoridor.ConsoleApp.GameManager
{
    public class HumanAgentConsole : AIStrategy<Movement, IGameEnvironment, IPlayer>
    {
        private readonly TextReader _inputSrc;
        private readonly TextWriter _outputDest;
        private readonly ICommandParser _commandParser;

        public HumanAgentConsole(
            TextReader inputSrc,
            TextWriter outputDest,
            ICommandParser commandParser)
        {
            _inputSrc = inputSrc;
            _outputDest = outputDest;
            _commandParser = commandParser;
        }

        public override string Name => nameof(HumanAgentConsole);

        public override AIStrategyResult<Movement> BestMove(IGameEnvironment gameView, IPlayer player)
        {
            while (true)
            {
                try
                {
                    var line = _inputSrc.ReadLine();
                    var move = _commandParser.Parse(line);
                    return new AIStrategyResult<Movement> { BestMove = move };
                }
                catch(Exception ex)
                {
                    _outputDest.WriteLine(ex.Message);
                }
            }
        }
    }
}
