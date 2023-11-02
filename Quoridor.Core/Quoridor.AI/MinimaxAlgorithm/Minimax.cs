using System;
using Quoridor.AI.Interfaces;

namespace Quoridor.AI.MinimaxAlgorithm
{
    public class Minimax<TPlayer, TMove, TGame> : AIStrategy<TMove, TGame, TPlayer>
        where TMove : Movement
        where TGame : IGame<TPlayer, TMove>
    {
        private readonly int _depth;

        public Minimax(int depth)
        {
            if (depth <= 0)
                throw new Exception($"invalid depth '{depth}'");

            _depth = depth;
        }

        public override string Name => nameof(MinimaxAlgorithm);

        public override AIStrategyResult<TMove> BestMove(TGame game, TPlayer player)
        {
            if (game.IsTerminal)
                throw new Exception($"Game already over. Cannot perform minimax");
            if (player == null)
                throw new Exception($"No agent to get scores from");

            var bestMove = MinimaxStep(game, player, _depth);
            return bestMove;
        }

        private AIStrategyResult<TMove> MinimaxStep(TGame game, TPlayer player, int depth)
        {
            if (depth <= 0 || game.IsTerminal)
                return new AIStrategyResult<TMove> { Value = game.Evaluate(player) };

            var maximizingPlayer = player.Equals(game.CurrentPlayer);
            var bestScore = maximizingPlayer ? double.MinValue : double.MaxValue;

            var bestMove = new AIStrategyResult<TMove> { Value = bestScore };

            foreach(var move in game.GetValidMovesFor(player))
            {
                game.Move(player, move);

                var result = MinimaxStep(game, player, depth - 1);
                if ((maximizingPlayer && result.Value > bestScore) || (!maximizingPlayer && result.Value < bestScore)) {
                    bestMove.BestMove = move;
                    bestMove.Value = result.Value;
                }

                game.UndoMove(player, move);
            }

            return bestMove;
        }
    }
}
