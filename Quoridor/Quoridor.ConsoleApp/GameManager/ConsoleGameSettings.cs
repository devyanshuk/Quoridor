﻿using System.IO;
using System.Collections.Generic;

namespace Quoridor.ConsoleApp.GameManager
{
    public class ConsoleGameSettings
    {
        public bool Verbose { get; set; }
        public bool Simulate { get; set; }
        public bool BranchingFactor { get; set; }
        public int NumberOfSimulations { get; set; }
        public bool WaitForInput { get; set; }
        public TextWriter OutputDest { get; set; }
        public List<StrategyInfo> Strategies { get; set; }
    }
}
