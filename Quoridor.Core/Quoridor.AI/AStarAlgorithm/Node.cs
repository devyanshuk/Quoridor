using System;
using Quoridor.AI.Interfaces;

namespace Quoridor.AI.AStarAlgorithm
{
    public class Node<TVector2D>
        where TVector2D : class, IVector2D
    {
        public TVector2D CurrPos { get; set; }
        public double FValue { get; set; }
        public double GValue { get; set; }
        public double HValue { get; set; }
        public Node<TVector2D> Parent { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            var node = obj as Node<TVector2D>;
            return CurrPos.X == node.CurrPos.X && CurrPos.Y == node.CurrPos.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CurrPos.X, CurrPos.Y);
        }
    }
}
