using System;

using Quoridor.AI.Interfaces;
using Quoridor.AI.Extensions;

namespace Quoridor.AI.MCTS
{
    public class MonteCarloTreeSearch<TMove, TGame, TPlayer> : IAIStrategy<TMove, TGame, TPlayer>
        where TGame : IMCTSGame<TGame, TMove, TPlayer>
        where TPlayer : IEquatable<TPlayer>
    {
        private readonly int _simulations;
        private readonly INodeSelectionStrategy<TMove, TPlayer> _selectionStrategy;
        private readonly IAIStrategy<TMove, TGame, TPlayer> _moveStrategy;
        private readonly System.Random _random = new System.Random();

        public MonteCarloTreeSearch(
            int simulations,
            INodeSelectionStrategy<TMove, TPlayer> selectionStrategy,
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

            var root = new Node<TMove, TPlayer> { Player = player };

            for(int i = 0; i < _simulations; i++)
            {
                var copy = game.DeepCopy();

                var selectedNode = Selection(copy, root);
                var expandedNode = Expansion(copy, selectedNode);
                var winner = Simulation(copy);
                BackPropagation(expandedNode, winner);
            }
            var bestChild = root.Children.MaxBy(c => c.Wins);
            var winRate = (float)bestChild.Wins / bestChild.Count;
            return new AIStrategyResult<TMove> { BestMove = bestChild.Move, Value = winRate };
        }

        private Node<TMove, TPlayer> Selection(TGame game, Node<TMove, TPlayer> node)
        {
            var currNode = node;

            while (!currNode.IsLeaf)
            {
                currNode = _selectionStrategy.PromisingNode(currNode);
                game.Move(currNode.Move);
            }

            return currNode;
        }

        private Node<TMove, TPlayer> Expansion(TGame game, Node<TMove, TPlayer> node)
        {
            if (game.HasFinished) return node;

            foreach(var move in game.GetValidMoves())
            {
                var childNode = new Node<TMove, TPlayer>
                {
                    Move = move,
                    Parent = node,
                    Player = game.Opponent
                };
                node.Children.Add(childNode);
            }
            var selectedChild = node.Children[_random.Next(node.Children.Count)];
            game.Move(selectedChild.Move);
            return selectedChild;
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

        private void BackPropagation(Node<TMove, TPlayer> node, TPlayer winner)
        {
            var current = node;
            while (current != null)
            {
                current.Count++;
                if (current.Player.Equals(winner))
                    current.Wins++;

                current = current.Parent;
            }
        }
    }
}
