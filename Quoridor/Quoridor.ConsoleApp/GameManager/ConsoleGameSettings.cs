using System.IO;
using System.Collections.Generic;

using Quoridor.Core.Game;
using Quoridor.AI.Interfaces;
using Quoridor.Core.Entities;

namespace Quoridor.ConsoleApp.GameManager
{
    public class ConsoleGameSettings
    {
        public bool WaitForInput { get; set; }
        public TextWriter OutputDest { get; set; }
        public List<AIStrategy<Movement, IGameEnvironment, IPlayer>> Strategies { get; set; }
    }
}
