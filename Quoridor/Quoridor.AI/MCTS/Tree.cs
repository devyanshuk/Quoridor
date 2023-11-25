namespace Quoridor.AI.MCTS
{
    public class Tree<TMove, TPlayer>
    {
        public Node<TMove, TPlayer> Root { get; set; }
    }
}
