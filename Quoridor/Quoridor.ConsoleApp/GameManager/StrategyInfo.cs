using System;
using Quoridor.AI.Interfaces;
using Quoridor.Core.Entities;
using Quoridor.Core.Game;

namespace Quoridor.ConsoleApp.GameManager
{
    public class StrategyInfo
    {
        public AIStrategy<Movement, IGameEnvironment, IPlayer> Strategy { get; set; }
        public int GamesWon { get; set; }
    }
}
