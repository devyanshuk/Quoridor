using System;

using Quoridor.AI.Extensions;

namespace Quoridor.AI.MCTS
{
    public class UCT<TMove, TPlayer, TGame> : INodeSelectionStrategy<TMove, TPlayer, TGame>
        where TGame : IMCTSGame<TGame, TMove, TPlayer>
    {
        private readonly double _c;

        public UCT(double c)
        {
            _c = c;
        }

        public Node<TMove, TPlayer, TGame> PromisingNode(Node<TMove, TPlayer, TGame> node)
        {
            return node.Children.MaxBy(n => Value(n));
        }

        private double Value(Node<TMove, TPlayer, TGame> node)
        {
            var winRate = (double)node.Wins / node.Count;
            return winRate + _c * Math.Log((double)node.Parent.Count / node.Count);
        }
    }
}
