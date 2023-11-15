using System;

namespace Quoridor.AI.AStarAlgorithm
{
    public class Node<TMove>
    {
        public TMove CurrMove { get; set; }
        public double FValue { get; set; }
        public double GValue { get; set; }
        public double HValue { get; set; }
        public Node<TMove> Parent { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            var node = obj as Node<TMove>;
            return CurrMove.Equals(node.CurrMove);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CurrMove);
        }
    }
}
