using System.Collections.Generic;

namespace Quoridor.AI.Interfaces
{
    public interface IRandomizableMoves<TMove>
    {
        public IEnumerable<TMove> RandomizableMoves();
    }
}
