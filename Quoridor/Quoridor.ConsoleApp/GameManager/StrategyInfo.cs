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
        public int SingleGameMoves { get; set; }
        public int SingleGameStates { get; set; }
    }
}
