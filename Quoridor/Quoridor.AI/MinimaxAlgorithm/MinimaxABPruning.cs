﻿using System;
using Quoridor.AI.Interfaces;

namespace Quoridor.AI.MinimaxAlgorithm
{
    public class MinimaxABPruning<TPlayer, TMove, TGame>(int Depth) : IAIStrategy<TMove, TGame, TPlayer>
        where TGame : IMinimaxGame<TPlayer, TMove>
    {
        public string Name => $"{nameof(MinimaxAlgorithm)}ABPruning";

        public AIStrategyResult<TMove> BestMove(TGame game, TPlayer player)
        {
            if (Depth <= 0)
                throw new Exception($"invalid depth '{Depth}'");
            if (game.HasFinished)
                throw new Exception($"Game already over. Cannot perform minimax");
            if (player == null)
                throw new Exception($"No agent to get scores from");

            var bestMove = MinimaxStep(game, double.MinValue, double.MaxValue,  Depth, player.Equals(game.CurrentPlayer));
            return bestMove;
        }

        private AIStrategyResult<TMove> MinimaxStep(TGame game, double alpha, double beta, int depth, bool maximizingPlayer)
        {
            if (depth <= 0 || game.HasFinished)
            {
                return new() { Value = game.Evaluate(maximizingPlayer) };
            }

            var bestMove = new AIStrategyResult<TMove> {
                Value = maximizingPlayer ? double.MinValue : double.MaxValue };

            foreach (var move in game.GetValidMoves())
            {
                game.Move(move);

                var result = MinimaxStep(game, alpha, beta, depth - 1, !maximizingPlayer);

                game.UndoMove(move);

                if (maximizingPlayer)
                {
                    if (result.Value > bestMove.Value)
                    {
                        bestMove.BestMove = move;
                        bestMove.Value = result.Value;
                    }
                    if (bestMove.Value > beta)
                        break;

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
                        break;

                    beta = Math.Min(beta, bestMove.Value);
                }
            }
            return bestMove;
        }
    }
}
