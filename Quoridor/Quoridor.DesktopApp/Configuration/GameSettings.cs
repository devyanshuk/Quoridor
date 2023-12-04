using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Quoridor.DesktopApp.Configuration
{
    [Serializable]
    public class GameSettings
    {
        [XmlElement(nameof(Players))]
        public int Players { get; set; }

        [XmlElement(nameof(Walls))]
        public int Walls { get; set; }

        [XmlElement(nameof(Dimension))]
        public int Dimension { get; set; }

        [XmlArray(nameof(Strategies))]
        [XmlArrayItem(nameof(MctsStrategy), typeof(MctsStrategy))]
        [XmlArrayItem(nameof(HumanStrategy), typeof(HumanStrategy))]
        [XmlArrayItem(nameof(AStarStrategy), typeof(AStarStrategy))]
        [XmlArrayItem(nameof(RandomStrategy), typeof(RandomStrategy))]
        [XmlArrayItem(nameof(GreedyStrategy), typeof(GreedyStrategy))]
        [XmlArrayItem(nameof(MinimaxStrategy), typeof(MinimaxStrategy))]
        [XmlArrayItem(nameof(MinimaxABStrategy), typeof(MinimaxABStrategy))]
        [XmlArrayItem(nameof(ParallelMinimaxABStrategy), typeof(ParallelMinimaxABStrategy))]
        public List<Strategy> Strategies { get; set; }

        [XmlArray(nameof(SelectedStrategies))]
        [XmlArrayItem(nameof(MctsStrategy), typeof(MctsStrategy))]
        [XmlArrayItem(nameof(HumanStrategy), typeof(HumanStrategy))]
        [XmlArrayItem(nameof(AStarStrategy), typeof(AStarStrategy))]
        [XmlArrayItem(nameof(RandomStrategy), typeof(RandomStrategy))]
        [XmlArrayItem(nameof(GreedyStrategy), typeof(GreedyStrategy))]
        [XmlArrayItem(nameof(MinimaxStrategy), typeof(MinimaxStrategy))]
        [XmlArrayItem(nameof(MinimaxABStrategy), typeof(MinimaxABStrategy))]
        [XmlArrayItem(nameof(ParallelMinimaxABStrategy), typeof(ParallelMinimaxABStrategy))]
        public List<Strategy> SelectedStrategies { get; set; }

        [XmlIgnore]
        public int Turn { get; set; } = 0;

        [XmlIgnore]
        public Strategy CurrentStrategy => SelectedStrategies[Turn];

        [XmlArray(nameof(Moves))]
        [XmlArrayItem(nameof(WallMove), typeof(WallMove))]
        [XmlArrayItem(nameof(PlayerMove), typeof(PlayerMove))]
        public List<MoveInfo> Moves { get; set; }

        public void NextTurn()
        {
            Turn = (Turn + 1) % SelectedStrategies.Count;
        }
    }
}
