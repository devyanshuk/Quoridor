﻿using System;
using Quoridor.AI.Interfaces;

namespace Quoridor.AI.MinimaxAlgorithm
{
    public class Minimax<TPlayer, TMove, TGame>(int Depth) : IAIStrategy<TMove, TGame, TPlayer>
        where TGame : IMinimaxGame<TPlayer, TMove>
    {
        public string Name => nameof(MinimaxAlgorithm);

        public AIStrategyResult<TMove> BestMove(TGame game, TPlayer player)
        {
            if (Depth <= 0)
                throw new Exception($"invalid depth '{Depth}'");
            if (game.HasFinished)
                throw new Exception($"Game already over. Cannot perform minimax");
            if (player == null)
                throw new Exception($"No agent to get scores from");

            var bestMove = MinimaxStep(game, Depth, player.Equals(game.CurrentPlayer));
            return bestMove;
        }

        private AIStrategyResult<TMove> MinimaxStep(TGame game, int depth, bool maximizingPlayer)
        {
            if (depth <= 0 || game.HasFinished)
                return new AIStrategyResult<TMove> { Value = game.Evaluate(maximizingPlayer) };

            var bestScore = maximizingPlayer ? double.MinValue : double.MaxValue;

            var bestMove = new AIStrategyResult<TMove> { Value = bestScore };

            foreach(var move in game.GetValidMoves())
            {
                game.Move(move);

                var result = MinimaxStep(game, depth - 1, !maximizingPlayer);
                if ((maximizingPlayer && result.Value > bestMove.Value) || (!maximizingPlayer && result.Value < bestMove.Value)) {
                    bestMove.BestMove = move;
                    bestMove.Value = result.Value;
                }
                game.UndoMove(move);
            }
            return bestMove;
        }
    }
}
