using Quoridor.Core.Environment;
using Quoridor.Core.Utils;

namespace Quoridor.Core.Game
{
    public class AffectedCell
    {
        public Cell Cell { get; set; }
        public Direction BlockedDirection { get; set; }
    }
}
