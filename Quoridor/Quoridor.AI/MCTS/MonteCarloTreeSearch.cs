using System;
using Quoridor.AI.Interfaces;
using Quoridor.AI.MinimaxAlgorithm;

namespace Quoridor.AI.MCTS
{
    public class MonteCarloTreeSearch<TMove, TGame, TPlayer> : AIStrategy<TMove, TGame, TPlayer>
        where TGame : IGame<TPlayer, TMove>, IDeepCopy<TGame>
    {
        public MonteCarloTreeSearch()
        {
        }

        public override string Name => nameof(MCTS);

        public override AIStrategyResult<TMove> BestMove(TGame game, TPlayer player)
        {
            throw new NotImplementedException();
        }
    }
}
