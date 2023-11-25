using System;

using Quoridor.AI.Interfaces;
using Quoridor.AI.MinimaxAlgorithm;

namespace Quoridor.AI.MCTS
{
    public class MonteCarloTreeSearch<TMove, TGame, TPlayer> : IAIStrategy<TMove, TGame, TPlayer>
        where TGame : IGame<TPlayer, TMove>, IDeepCopy<TGame>
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
            throw new NotImplementedException();
        }
    }
}
