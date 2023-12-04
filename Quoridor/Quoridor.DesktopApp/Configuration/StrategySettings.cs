using System;
using System.Xml.Serialization;

using Quoridor.Core;
using Quoridor.AI.MCTS;
using Quoridor.AI.Random;
using Quoridor.Core.Game;
using Quoridor.AI.Interfaces;
using Quoridor.Core.Entities;
using Quoridor.AI.AStarAlgorithm;
using Quoridor.AI.MinimaxAlgorithm;

namespace Quoridor.DesktopApp.Configuration
{
    [XmlInclude(typeof(MctsStrategy))]
    [XmlInclude(typeof(HumanStrategy))]
    [XmlInclude(typeof(AStarStrategy))]
    [XmlInclude(typeof(RandomStrategy))]
    [XmlInclude(typeof(GreedyStrategy))]
    [XmlInclude(typeof(MinimaxStrategy))]
    [XmlInclude(typeof(MinimaxABStrategy))]
    [XmlInclude(typeof(ParallelMinimaxABStrategy))]
    public abstract class Strategy
    {
        [XmlAttribute(nameof(Description))]
        public string Description { get; set; }

        public virtual IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy() => null;

        public override string ToString() => Description;

        public virtual string GetParams() => string.Empty;
    }

    public abstract class NonParamStrategy : Strategy { }

    [Serializable]
    public class HumanStrategy : NonParamStrategy { }

    [Serializable]
    public class AStarStrategy : NonParamStrategy
    {
        [XmlIgnore]
        private AStar<Movement, IGameEnvironment, IPlayer> _astar;

        public override IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy()
        {
            return _astar ??= new();
        }
    }

    [XmlInclude(typeof(RandomStrategy))]
    [XmlInclude(typeof(GreedyStrategy))]
    public abstract class MCTSAgent : Strategy
    {
        [XmlAttribute(nameof(Seed))]
        public int Seed { get; set; }
    }

    [Serializable]
    public class RandomStrategy : MCTSAgent
    {
        [XmlIgnore]
        private RandomStrategy<Movement, IGameEnvironment, IPlayer> _randomStrategy;

        public override IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy()
        {
            return _randomStrategy ??= new(Seed);
        }

        public override string GetParams()
        {
            return $"{nameof(Seed)}:{Seed}";
        }
    }

    [Serializable]
    public class GreedyStrategy : MCTSAgent
    {
        [XmlIgnore]
        private GreedyStrategy<IGameEnvironment, Movement, IPlayer> _greedyStrategy;

        public override IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy()
        {
            return _greedyStrategy ??= new(Seed);
        }

        public override string GetParams()
        {
            return $"{nameof(Seed)}:{Seed}";
        }
    }

    [Serializable]
    public class MctsStrategy : Strategy
    {
        [XmlAttribute(nameof(C))]
        public double C { get; set; }

        [XmlAttribute(nameof(Simulations))]
        public int Simulations { get; set; }

        [XmlElement(nameof(RandomStrategy), typeof(RandomStrategy))]
        [XmlElement(nameof(GreedyStrategy), typeof(GreedyStrategy))]
        public MCTSAgent SimulationStrategy { get; set; }

        [XmlIgnore]
        private MonteCarloTreeSearch<Movement, IGameEnvironment, IPlayer> _mctsStrategy;

        [XmlIgnore]
        private UCT<Movement, IPlayer, IGameEnvironment> _uctSelection;

        public override IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy()
        {
            if (_uctSelection == null) _uctSelection = new(C);
            return _mctsStrategy ??= new(Simulations, _uctSelection, SimulationStrategy.GetStrategy());
        }

        public override string GetParams()
        {
            return $"{nameof(C)}:{C}, Sim:{Simulations}, {nameof(Strategy)}={SimulationStrategy}";
        }
    }

    [Serializable]
    public class MinimaxStrategy : Strategy
    {
        [XmlAttribute(nameof(Depth))]
        public int Depth { get; set; }

        [XmlIgnore]
        private Minimax<IPlayer, Movement, IGameEnvironment> _minimax;

        public override IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy()
        {
            return _minimax ??= new(Depth);
        }

        public override string GetParams()
        {
            return $"{nameof(Depth)}:{Depth}";
        }
    }

    [Serializable]
    public class MinimaxABStrategy : MinimaxStrategy
    {
        [XmlIgnore]
        private MinimaxABPruning<IPlayer, Movement, IGameEnvironment> _minimaxAB;

        public override IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy()
        {
            return _minimaxAB ??= new(Depth);
        }
    }

    [Serializable]
    public class ParallelMinimaxABStrategy : MinimaxStrategy
    {
        [XmlIgnore]
        private ParallelMinimaxABPruning<IPlayer, Movement, IGameEnvironment> _parallelMinimaxAB;

        public override IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy()
        {
            return _parallelMinimaxAB ??= new(Depth);
        }
    }
}
