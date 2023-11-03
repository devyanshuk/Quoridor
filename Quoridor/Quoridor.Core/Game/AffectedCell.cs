using Quoridor.Core.Utils;
using Quoridor.Core.Environment;

namespace Quoridor.Core.Game
{
    public class AffectedCell
    {
        public Cell Cell { get; }
        public Direction BlockedDirection { get; }

        public AffectedCell(Cell cell, Direction blockedDirection)
        {
            Cell = cell;
            BlockedDirection = blockedDirection;
        }
    }
}
