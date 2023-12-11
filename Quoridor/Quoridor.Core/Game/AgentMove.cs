using System;
using Quoridor.Core.Utils;

namespace Quoridor.Core.Game
{
    public sealed class AgentMove : Movement, IEquatable<AgentMove>
    {
        public Vector2 NewPos { get; private set; }
        //The direction the new move is at
        public Direction Dir { get; private set; }
        public Vector2 CurrentPos { get; internal set; }

        public AgentMove(Direction dir, Vector2 newPos) {
            Dir = dir;
            NewPos = newPos;
        }

        public bool Equals(AgentMove other)
        {
            if (other is null) return false;
            return Dir.Equals(other.Dir) && CurrentPos.Equals(other.CurrentPos) && NewPos.Equals(other.NewPos);
        }

        public override string ToString()
        {
            return $"{Dir}ern direction move to {NewPos}";
        }
    }
}
