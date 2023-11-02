using System;
using Quoridor.AI.Interfaces;

namespace Quoridor.AI.MinimaxAlgorithm
{
    public class Minimax<TPlayer, TMove, TGame> : AIAgent<TMove, TGame, TPlayer>
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

        public override TMove BestMove(TGame game, TPlayer player)
        {
            if (game.IsTerminal)
                throw new Exception($"Game already over. Cannot perform minimax");
            if (player == null)
                throw new Exception($"No agent to get scores from");

            var bestMove = MinimaxStep(game, player, _depth);
            return bestMove.Item1;
        }

        private Tuple<TMove, double> MinimaxStep(TGame game, TPlayer player, int depth)
        {
            if (depth <= 0 || game.IsTerminal)
                return Tuple.Create(default(TMove), game.Evaluate(player));

            var maximizingPlayer = player.Equals(game.CurrentPlayer);
            var bestScore = maximizingPlayer ? double.MinValue : double.MaxValue;
            var bestMove = Tuple.Create(default(TMove), bestScore);

            foreach(var move in game.GetValidMovesFor(player))
            {
                game.Move(move);

                var currScore = MinimaxStep(game, player, depth - 1);
                if ((maximizingPlayer && currScore.Item2 > bestScore) || (!maximizingPlayer && currScore.Item2 < bestScore))
                    bestMove = Tuple.Create(move, currScore.Item2);

                game.UndoMove(move);
            }

            return bestMove;
        }
    }
}
