using System;
using System.Linq;

namespace Quoridor.AI.MCTS
{
    public class UCT<TMove, TPlayer, TGame>(double c) : INodeSelectionStrategy<TMove, TPlayer, TGame>
        where TGame : IMCTSGame<TGame, TMove, TPlayer>
    {
        public Node<TMove, TPlayer, TGame> PromisingNode(Node<TMove, TPlayer, TGame> node)
        {
            return node.Children.MaxBy(Value);
        }

        private double Value(Node<TMove, TPlayer, TGame> node)
        {
            var winRate = (double)node.Wins / node.Count;
            return winRate + c * Math.Log((double)node.Parent.Count / node.Count);
        }
    }
}
