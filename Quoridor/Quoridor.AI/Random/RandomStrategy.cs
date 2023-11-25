using System.Linq;

using Quoridor.AI.Interfaces;

namespace Quoridor.AI.Random
{
    public class RandomStrategy<TMove, TGame, TPlayer>  : IAIStrategy<TMove, TGame, TPlayer>
        where TGame : IValidMoves<TMove>
    {
        private readonly System.Random _random;
        private readonly int _seed;

        public RandomStrategy(int seed)
        {
            _seed = seed;
            _random = new System.Random(seed);
        }

        public string Name => nameof(Random);

        public AIStrategyResult<TMove> BestMove(TGame game, TPlayer player)
        {
            var validMoves = game.GetValidMoves();
            var randIndex = _random.Next(0, validMoves.Count());
            return new AIStrategyResult<TMove> { BestMove = validMoves.ElementAt(randIndex), Value = _seed };
        }
    }
}
