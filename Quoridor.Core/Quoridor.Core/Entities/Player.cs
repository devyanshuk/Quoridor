using Quoridor.Core.Utils;

namespace Quoridor.Core.Entities
{
    public class Player : IPlayer
    {
        public int NumWalls { get; set; }
        public char Id { get; }
        public Vector2 StartPos { get; }
        public Vector2 CurrentPos { get; set; }

        public Player(char id, int numWalls, Vector2 startPos)
        {
            Id = id;
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
