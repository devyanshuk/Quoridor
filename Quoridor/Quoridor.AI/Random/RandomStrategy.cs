using System.Linq;

using Quoridor.AI.Interfaces;

namespace Quoridor.AI.Random
{
    public class RandomStrategy<TMove, TGame, TPlayer>(int seed)  : IAIStrategy<TMove, TGame, TPlayer>
        where TGame : IValidMoves<TMove>
    {
        private readonly System.Random _random = new(seed);

        public string Name => nameof(Random);

        public AIStrategyResult<TMove> BestMove(TGame game, TPlayer player)
        {
            var validMoves = game.GetValidMoves();
            var randIndex = _random.Next(0, validMoves.Count());
            return new() { BestMove = validMoves.ElementAt(randIndex), Value = seed };
        }
    }
}
