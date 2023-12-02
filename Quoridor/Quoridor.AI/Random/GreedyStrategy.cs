using System.Linq;

using Quoridor.AI.Interfaces;
using Quoridor.AI.AStarAlgorithm;

namespace Quoridor.AI.Random
{
    public class GreedyStrategy<TGame, TMove, TPlayer> : IAIStrategy<TMove, TGame, TPlayer>
        where TPlayer : IAStarPlayer<TMove>
        where TGame : INeighbors<TMove>, INonPlayerMovableMove<TMove>
    {
        public string Name => "Greedy";

        private readonly AStar<TMove, TGame, TPlayer> _aStar;
        private readonly System.Random _random;
        private readonly int _seed;

        public GreedyStrategy(int seed)
        {
            _seed = seed;
            _random = new(seed);
            _aStar = new();
        }

        public AIStrategyResult<TMove> BestMove(TGame game, TPlayer player)
        {
            var possibleMoves = game.NonPlayerMovableMoves().ToList();
            var bestWalkableMove = _aStar.BestMove(game, player).BestMove;
            possibleMoves.Add(bestWalkableMove);
            var randIndex = _random.Next(0, possibleMoves.Count);
            return new AIStrategyResult<TMove> { BestMove = possibleMoves[randIndex], Value = _seed };
        }
    }
}
