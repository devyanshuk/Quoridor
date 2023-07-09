using System;
using Quoridor.Core.Environment;

namespace Quoridor.Core.Entities
{
    public class Player
    {
        public int NumWalls { get; set; }

        public Cell Cell { get; private set; }
        

        public Player(int numWalls)
        {
            NumWalls = numWalls;
        }
    }
}
