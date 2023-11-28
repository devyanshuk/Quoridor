namespace Quoridor.AI.MCTS
{
    public interface INodeSelectionStrategy<TMove, TPlayer, TGame>
        where TGame : IMCTSGame<TGame, TMove, TPlayer>
    {
        Node<TMove, TPlayer, TGame> PromisingNode(Node<TMove, TPlayer, TGame> node);
    }
}
