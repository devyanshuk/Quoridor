using System;
using Quoridor.AI.Interfaces;

namespace Quoridor.AI.MinimaxAlgorithm
{
    public class ParallelMinimaxABPruning<TPlayer, TMove, TGame> : AIStrategy<TMove, TGame, TPlayer>
        where TMove : Movement
        where TGame : IGame<TPlayer, TMove>, IDeepCopy<TGame>
    {
        private readonly int _depth;

        public ParallelMinimaxABPruning(int depth)
        {
            _depth = depth;
        }

        public override string Name => $"Parallel {nameof(MinimaxAlgorithm)}ABPruning";

        public override AIStrategyResult<TMove> BestMove(TGame game, TPlayer player)
        {
            if (game.HasFinished)
                throw new Exception($"Game already over. Cannot perform minimax");
            if (player == null)
                throw new Exception($"No agent to get scores from");

            var bestMove = MinimaxStep(game, double.MinValue, double.MaxValue, _depth, player.Equals(game.CurrentPlayer));
            return bestMove;
        }

        private AIStrategyResult<TMove> MinimaxStep(TGame game, double alpha, double beta, int depth, bool maximizingPlayer)
        {
            return null;
        }
    }
}
