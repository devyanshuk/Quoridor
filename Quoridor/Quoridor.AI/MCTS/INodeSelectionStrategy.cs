namespace Quoridor.AI.MCTS
{
    public interface INodeSelectionStrategy<TMove, TPlayer>
    {
        Node<TMove, TPlayer> PromisingNode(Node<TMove, TPlayer> node);
    }
}
