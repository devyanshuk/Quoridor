using System;
using System.Linq;
using System.Threading.Tasks;
using Quoridor.AI.Interfaces;

namespace Quoridor.AI.MinimaxAlgorithm
{
    public class ParallelMinimaxABPruning<TPlayer, TMove, TGame> : AIStrategy<TMove, TGame, TPlayer>
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

            var bestMove = ParallelMinimaxStep(game, double.MinValue, double.MaxValue, _depth, player.Equals(game.CurrentPlayer));
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

            var locker = new object();

            Parallel.ForEach(game.GetValidMoves(), (move, loopState) =>
            {
                var clonedGame = game.DeepCopy();
                clonedGame.Move(move);

                var result = ParallelMinimaxStep(clonedGame, alpha, beta, depth - 1, !maximizingPlayer);

                lock (locker)
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
