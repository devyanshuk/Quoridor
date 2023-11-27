using System.Collections.Generic;

namespace Quoridor.AI.MCTS
{
    public class Node<TMove, TPlayer>
    {
        public int Count { get; set; }
        public int Wins { get; set; }
        public Node<TMove, TPlayer> Parent { get; set; }
        public List<Node<TMove, TPlayer>> Children { get; set; }

        public TPlayer Player { get; set; }
        public TMove Move { get; set; }

        public bool IsLeaf => Children.Count == 0;
    }
}
