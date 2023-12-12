using System.Linq;

using Quoridor.AI.Interfaces;
using Quoridor.AI.AStarAlgorithm;

namespace Quoridor.AI.Random
{
    public class SemiRandomStrategy<TGame, TMove, TPlayer> : IAIStrategy<TMove, TGame, TPlayer>
        where TPlayer : IAStarPlayer<TMove>
        where TGame : INeighbors<TMove>, IRandomizableMoves<TMove>
    {
        public string Name => "Semi-Random";

        private readonly int _seed;
        private readonly System.Random _random;

        private readonly IAIStrategy<TMove, TGame, TPlayer> _strategy;

        public SemiRandomStrategy(int seed, IAIStrategy<TMove, TGame, TPlayer> strategy)
        {
            _seed = seed;
            _random = new(seed);
            _strategy = strategy;
        }

        public AIStrategyResult<TMove> BestMove(TGame game, TPlayer player)
        {
            var possibleMoves = game.RandomizableMoves().ToList();
            var bestWalkableMove = _strategy.BestMove(game, player).BestMove;
            possibleMoves.Add(bestWalkableMove);
            var randIndex = _random.Next(0, possibleMoves.Count);
            return new() { BestMove = possibleMoves[randIndex], Value = _seed };
        }
    }
}
