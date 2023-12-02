using System.Collections.Generic;

namespace Quoridor.AI.Interfaces
{
    public interface INonPlayerMovableMove<TMove>
    {
        public IEnumerable<TMove> NonPlayerMovableMoves();
    }
}
