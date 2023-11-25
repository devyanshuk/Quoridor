using Quoridor.Core;
using Quoridor.Core.Game;
using Quoridor.AI.Interfaces;
using Quoridor.Core.Entities;

namespace Quoridor.ConsoleApp.GameManager
{
    public class StrategyInfo
    {
        public IAIStrategy<Movement, IGameEnvironment, IPlayer> Strategy { get; set; }
        public int GamesWon { get; set; }
    }
}
