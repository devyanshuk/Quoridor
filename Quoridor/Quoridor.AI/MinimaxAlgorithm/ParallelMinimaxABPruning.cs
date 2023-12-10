using System;
using System.Linq;
using System.Threading.Tasks;

using Quoridor.AI.Interfaces;

namespace Quoridor.AI.MinimaxAlgorithm
{
    public class ParallelMinimaxABPruning<TPlayer, TMove, TGame>(int Depth) : IAIStrategy<TMove, TGame, TPlayer>
        where TGame : IMinimaxGame<TPlayer, TMove>, IDeepCopy<TGame>
    {
        private readonly object _lock = new();

        public string Name => $"Parallel {nameof(MinimaxAlgorithm)}ABPruning";

        public AIStrategyResult<TMove> BestMove(TGame game, TPlayer player)
        {
            if (Depth <= 0)
                throw new Exception($"invalid depth '{Depth}'");
            if (game.HasFinished)
                throw new Exception($"Game already over. Cannot perform minimax");
            if (player == null)
                throw new Exception($"No agent to get scores from");

            var bestMove = ParallelMinimaxStep(game, double.MinValue, double.MaxValue, Depth, player.Equals(game.CurrentPlayer));
            return bestMove;
        }

        private AIStrategyResult<TMove> ParallelMinimaxStep(TGame game, double alpha, double beta, int depth, bool maximizingPlayer)
        {
            if (depth <= 0 || game.HasFinished)
            {
                var score = game.Evaluate(maximizingPlayer);
                return new AIStrategyResult<TMove> { Value = score };
            }

            var bestMove = new AIStrategyResult<TMove>
            {
                Value = maximizingPlayer ? double.MinValue : double.MaxValue
            };

            var validMoves = game.GetValidMoves().ToList();

            Parallel.ForEach(validMoves, (move, loopState) =>
            {
                var clonedGame = game.DeepCopy();
                clonedGame.Move(move);

                var result = ParallelMinimaxStep(clonedGame, alpha, beta, depth - 1, !maximizingPlayer);

                lock (_lock)
                {
                    if (maximizingPlayer)
                    {
                        if (result.Value > bestMove.Value)
                        {
                            bestMove.BestMove = move;
                            bestMove.Value = result.Value;
                        }
                        if (bestMove.Value > beta)
                            loopState.Break();

                        alpha = Math.Max(alpha, bestMove.Value);
                    }
                    else
                    {
                        if (result.Value < bestMove.Value)
                        {
                            bestMove.BestMove = move;
                            bestMove.Value = result.Value;
                        }
                        if (bestMove.Value < alpha)
                            loopState.Break();

                        beta = Math.Min(beta, bestMove.Value);
                    }
                }
            });
            return bestMove;
        }
    }
}
