using Quoridor.Core.Utils;

namespace Quoridor.Core.Entities
{
    public class Player : IPlayer
    {
        public int NumWalls { get; set; }

        public Vector2 StartPos { get; }
        public Vector2 CurrentPos { get; set; }

        public Player(int numWalls, Vector2 startPos)
        {
            NumWalls = numWalls;
            StartPos = startPos;
            CurrentPos = startPos;
        }

        public void Move(Vector2 newPos)
        {
            CurrentPos = newPos;
        }
    }
}
