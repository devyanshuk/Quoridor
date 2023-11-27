using System;

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
            throw new NotImplementedException();
        }
    }
}
