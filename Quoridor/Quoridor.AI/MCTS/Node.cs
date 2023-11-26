using System.Collections.Generic;

namespace Quoridor.AI.MCTS
{
    public class Node<TMove, TPlayer>
    {
        public TPlayer Player { get; set; }
        public Node<TMove, TPlayer> Parent { get; set; }
        public List<Node<TMove, TPlayer>> Children { get; set; } = new List<Node<TMove, TPlayer>>();

        public bool IsLeaf => Children.Count == 0;
    }
}
