using Quoridor.Core.Utils;

namespace Quoridor.Core.Entities
{
    public interface IPlayer
    {
        public char Id { get; }
        public int NumWalls { get; set; }
        public Vector2 StartPos { get; }
        public Vector2 CurrentPos { get; set; }

        void Move(Vector2 newPos);
        void DecreaseWallCount();
    }
}
