using System.Collections.Generic;
using System.IO;

namespace Quoridor.ConsoleApp.GameManager
{
    public class ConsoleGameSettings
    {
        public int NumWalls { get; set; }
        public int NumPlayers { get; set; }

        public TextReader InputSrc { get; set; }
        public TextWriter OutputDest { get; set; }
    }
}
