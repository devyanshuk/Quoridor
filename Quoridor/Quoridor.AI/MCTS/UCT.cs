using System;

using Quoridor.AI.Extensions;

namespace Quoridor.AI.MCTS
{
    public class UCT<TMove, TPlayer> : INodeSelectionStrategy<TMove, TPlayer>
    {
        private readonly double _c;

        public UCT(double c)
        {
            _c = c;
        }

        public Node<TMove, TPlayer> PromisingNode(Node<TMove, TPlayer> node)
        {
            return node.Children.MaxBy(n => Value(n));
        }

        private double Value(Node<TMove, TPlayer> node)
        {
            var winRate = (double)node.Wins / node.Count;
            return winRate + _c * Math.Log((double)node.Parent.Count / node.Count);
        }
    }
}
