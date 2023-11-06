using System;
using Quoridor.AI.Interfaces;

namespace Quoridor.AI.MinimaxAlgorithm
{
    public class MinimaxABPruning<TPlayer, TMove, TGame> : AIStrategy<TMove, TGame, TPlayer>
        where TMove : Movement
        where TGame : IGame<TPlayer, TMove>
    {
        private readonly int _depth;

        public MinimaxABPruning(int depth)
        {
            _depth = depth;
        }

        public override string Name => $"{nameof(MinimaxAlgorithm)}ABPruning";

        public override AIStrategyResult<TMove> BestMove(TGame game, TPlayer player)
        {
            if (game.HasFinished)
                throw new Exception($"Game already over. Cannot perform minimax");
            if (player == null)
                throw new Exception($"No agent to get scores from");

            var bestMove = MinimaxStep(game, double.MinValue, double.MaxValue,  _depth, player.Equals(game.CurrentPlayer));
            return bestMove;
        }

        private AIStrategyResult<TMove> MinimaxStep(TGame game, double alpha, double beta, int depth, bool maximizingPlayer)
        {
            if (depth <= 0 || game.HasFinished)
                return new AIStrategyResult<TMove> { Value = game.Evaluate() };

            var bestScore = maximizingPlayer ? double.MinValue : double.MaxValue;
            Func<double, bool> cutoff_condition = (value) => maximizingPlayer ? value > beta : value < alpha;

            var bestMove = new AIStrategyResult<TMove> { Value = bestScore };

            foreach (var move in game.GetValidMoves())
            {
                game.Move(move);

                var result = MinimaxStep(game, alpha, beta, depth - 1, !maximizingPlayer);

                if ((maximizingPlayer && result.Value > bestMove.Value) || (!maximizingPlayer && result.Value < bestMove.Value))
                {
                    bestMove.BestMove = move;
                    bestMove.Value = result.Value;
                }

                if (cutoff_condition(bestMove.Value))
                {
                    game.UndoMove(move);
                    break;
                }

                if (maximizingPlayer)
                    alpha = Math.Max(alpha, bestMove.Value);
                else
                    beta = Math.Min(beta, bestMove.Value);

                game.UndoMove(move);
            }
            return bestMove;
        }
    }
}
