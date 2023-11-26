using System;

using Quoridor.AI.Interfaces;

namespace Quoridor.AI.MCTS
{
    public class MonteCarloTreeSearch<TMove, TGame, TPlayer> : IAIStrategy<TMove, TGame, TPlayer>
        where TGame : IMCTSGame<TGame, TMove, TPlayer>
    {
        //exploration parameter
        private readonly double _c;
        private readonly int _simulations;

        public MonteCarloTreeSearch(double c, int simulations)
        {
            _c = c;
            _simulations = simulations;
        }

        public string Name => nameof(MCTS);

        public AIStrategyResult<TMove> BestMove(TGame game, TPlayer player)
        {
            if (game.HasFinished)
                throw new Exception($"Game already over. Cannot perform minimax");
            if (player == null)
                throw new Exception($"No agent to get scores from");

            var root = new Node<TMove, TPlayer> { Player = player };

            for(int i = 0; i < _simulations; i++)
            {
                var selectedNode = Selection(root);
                var expandedNode = Expansion(selectedNode);
                var result = Simulation(expandedNode);
                BackPropagation(expandedNode, result);
            }

            return null;

        }

        private Node<TMove, TPlayer> Selection(Node<TMove, TPlayer> root)
        {
            return null;
        }

        private Node<TMove, TPlayer> Expansion(Node<TMove, TPlayer> node)
        {
            return null;
        }

        private double Simulation(Node<TMove, TPlayer> node)
        {
            return 0;
        }

        private void BackPropagation(Node<TMove, TPlayer> node, double result)
        {

        }
    }
}
