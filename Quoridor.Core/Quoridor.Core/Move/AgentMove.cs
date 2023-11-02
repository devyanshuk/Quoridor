using System;
using Quoridor.Core.Utils;

using Quoridor.AI.Interfaces;

namespace Quoridor.Core.Move
{
    public sealed class AgentMove : Movement, IEquatable<AgentMove>
    {
        public Direction Dir { get; private set; }

        public AgentMove(Direction dir) {
            Dir = dir;
        }

        public bool Equals(AgentMove other)
        {
            if (other is null) return false;
            return Dir.Equals(other.Dir);
        }
    }
}
