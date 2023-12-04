using System;
using Quoridor.Core.Utils;

namespace Quoridor.Core.Game
{
    public sealed class AgentMove : Movement, IEquatable<AgentMove>
    {
        public Direction Dir { get; private set; }
        public Vector2 CurrentPos { get; internal set; }

        public AgentMove(Direction dir) {
            Dir = dir;
        }

        public bool Equals(AgentMove other)
        {
            if (other is null) return false;
            return Dir.Equals(other.Dir) && CurrentPos.Equals(other.CurrentPos);
        }

        public override string ToString()
        {
            return $"{Dir}ern direction move";
        }
    }
}
