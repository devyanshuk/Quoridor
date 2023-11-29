using System;
using System.Linq;

using Quoridor.AI.Interfaces;

namespace Quoridor.AI.MCTS
{
    public class MonteCarloTreeSearch<TMove, TGame, TPlayer> : IAIStrategy<TMove, TGame, TPlayer>
        where TGame : IMCTSGame<TGame, TMove, TPlayer>
        where TPlayer : IEquatable<TPlayer>
    {
        private readonly int _simulations;
        private readonly INodeSelectionStrategy<TMove, TPlayer, TGame> _selectionStrategy;
        private readonly IAIStrategy<TMove, TGame, TPlayer> _moveStrategy;

        public MonteCarloTreeSearch(
            int simulations,
            INodeSelectionStrategy<TMove, TPlayer, TGame> selectionStrategy,
            IAIStrategy<TMove, TGame, TPlayer> moveStrategy)
        {
            _simulations = simulations;
            _selectionStrategy = selectionStrategy;
            _moveStrategy = moveStrategy;
        }

        public string Name => nameof(MCTS);

        public AIStrategyResult<TMove> BestMove(TGame game, TPlayer player)
        {
            if (game.HasFinished)
                throw new Exception($"Game already over. Cannot perform MCTS");
            if (player == null)
                throw new Exception($"No agent registered");

            var root = new Node<TMove, TPlayer, TGame>(game.DeepCopy());

            for (int i = 0; i < _simulations; i++)
            {
                var selectedNode = Selection_Extraction(root);
                var winner = Simulation(selectedNode.State.DeepCopy());
                BackPropagation(selectedNode, winner);
            }
            var bestChild = root.Children.MaxBy(c => (double)c.Wins/c.Count);
            var winRate = (float)bestChild.Wins / bestChild.Count;
            return new AIStrategyResult<TMove> { BestMove = bestChild.Move, Value = winRate };
        }

        private Node<TMove, TPlayer, TGame> Selection_Extraction(Node<TMove, TPlayer, TGame> node)
        {
            var currNode = node;

            while (!currNode.State.HasFinished)
            {
                if (currNode.Expandable)
                {
                    return currNode.Expand();
                }
                currNode = _selectionStrategy.PromisingNode(currNode);
            }

            return currNode;
        }


        private TPlayer Simulation(TGame game)
        {
            while(!game.HasFinished)
            {
                var move = _moveStrategy.BestMove(game, game.CurrentPlayer).BestMove;
                game.Move(move);
            }
            return game.Winner;
        }

        private void BackPropagation(Node<TMove, TPlayer, TGame> node, TPlayer winner)
        {
            var current = node;
            while (current != null)
            {
                current.Count++;
                if (current.State.Opponent.Equals(winner))
                    current.Wins++;

                current = current.Parent;
            }
        }
    }
}
