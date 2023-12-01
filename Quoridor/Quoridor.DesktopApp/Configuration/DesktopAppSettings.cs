using System;
using System.Drawing;
using System.Xml.Serialization;
using System.Collections.Generic;

using Quoridor.AI;
using Quoridor.Core;
using Quoridor.AI.MCTS;
using Quoridor.AI.Random;
using Quoridor.Core.Game;
using Quoridor.Core.Utils;
using Quoridor.AI.Interfaces;
using Quoridor.Core.Entities;
using Quoridor.Common.Helpers;
using Quoridor.Core.Environment;
using Quoridor.AI.MinimaxAlgorithm;
using Quoridor.AI.AStarAlgorithm;

namespace Quoridor.DesktopApp.Configuration
{
    [XmlRoot(nameof(DesktopAppSettings))]
    public class DesktopAppSettings
    {
        [XmlElement(nameof(FormSettings))]
        public FormSettings FormSettings { get; set; }

        [XmlElement(nameof(ColorSettings))]
        public ColorSettings ColorSettings { get; set; }

        [XmlElement(nameof(GameSettings))]
        public GameSettings GameSettings { get; set; }

        [XmlElement(nameof(FontSettings))]
        public FontSettings FontSettings { get; set; }

        public int CellSize
        {
            get
            {
                var totalWallWidth = FormSettings.WallWidth * (GameSettings.Dimension - 2);
                var totalCellSize = (FormSettings.ScreenWidth - 2 * FormSettings.OffsetX - totalWallWidth);
                var cellSize = totalCellSize / GameSettings.Dimension;
                return cellSize;
            }
        }

        public int WallHeight => CellSize * 2 + FormSettings.WallWidth;
    }

    [Serializable]
    public class FormSettings
    {
        [XmlAttribute(nameof(Description))]
        public string Description { get; set; }

        [XmlElement(nameof(Title))]
        public string Title { get; set; }

        [XmlElement(nameof(ScreenWidth))]
        public int _screenWidth { get; set; }

        [XmlElement(nameof(ScreenHeight))]
        public int _screenHeight { get; set; }

        [XmlElement(nameof(OffsetX))]
        public int OffsetX { get; set; }

        [XmlIgnore]
        private int _offsetY { get; set; } = 30;

        [XmlElement(nameof(OffsetY))]
        public int OffsetY
        {
            get { return _offsetY; }
            set
            {
                if (value < _offsetY)
                    throw new Exception($"{nameof(OffsetY)} must be atleast {_offsetY}");
                _offsetY = value;
            }
        }

        [XmlElement(nameof(WallWidth))]
        public int WallWidth { get; set; }

        [XmlIgnore]
        public int ScreenWidth => ValidateDimension(_screenWidth);

        [XmlIgnore]
        public int ScreenHeight => ValidateDimension(_screenHeight);

        public int ValidateDimension(int screenprop)
        {
            if (_screenHeight < _screenWidth)
                throw new Exception($"Screen height must be more than width");

            return screenprop;
        }
    }

    [Serializable]
    public class ColorSettings
    {
        [XmlElement(nameof(BackgroundColor))]
        public string _backgroundColor { get; set; }

        [XmlElement(nameof(OddTileColor))]
        public string _oddTileColor { get; set; }

        [XmlElement(nameof(PlayerStatsColor))]
        public string _playerStatsColor { get; set; }

        [XmlElement(nameof(EvenTileColor))]
        public string _evenTileColor { get; set; }

        [XmlElement(nameof(PlayerColor))]
        public string _playerColor { get; set; }

        [XmlElement(nameof(WallColor))]
        public string _wallColor { get; set; }

        [XmlElement(nameof(Opacity))]
        public int Opacity { get; set; }

        [XmlIgnore]
        public Color WallColor => Color.FromName(_wallColor);

        [XmlIgnore]
        public Color BackgroundColor => Color.FromName(_backgroundColor);

        [XmlIgnore]
        public Color PlayerColor => Color.FromName(_playerColor);

        [XmlIgnore]
        public Color PlayerStatsColor => Color.FromName(_playerStatsColor);

        [XmlIgnore]
        public Color OddTileColor => Color.FromArgb(Opacity, Color.FromName(_oddTileColor));

        [XmlIgnore]
        public Color EvenTileColor => Color.FromArgb(Opacity, Color.FromName(_evenTileColor));
    }

    [Serializable]
    public class FontSettings
    {
        [XmlElement(nameof(PlayerFont))]
        public string PlayerFont { get; set; }
    }

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
        [XmlArrayItem(nameof(HumanStrategy), typeof(HumanStrategy))]
        [XmlArrayItem(nameof(AStarStrategy), typeof(AStarStrategy))]
        [XmlArrayItem(nameof(RandomStrategy), typeof(RandomStrategy))]
        [XmlArrayItem(nameof(MctsStrategy), typeof(MctsStrategy))]
        [XmlArrayItem(nameof(MinimaxStrategy), typeof(MinimaxStrategy))]
        [XmlArrayItem(nameof(MinimaxABStrategy), typeof(MinimaxABStrategy))]
        [XmlArrayItem(nameof(ParallelMinimaxABStrategy), typeof(ParallelMinimaxABStrategy))]
        public List<Strategy> Strategies { get; set; }

        [XmlIgnore]
        public int Turn { get; set; } = 0;

        [XmlArray(nameof(Moves))]
        [XmlArrayItem(nameof(WallMove), typeof(WallMove))]
        [XmlArrayItem(nameof(PlayerMove), typeof(PlayerMove))]
        public List<MoveInfo> Moves { get; set; }

        public void NextTurn()
        {
            Turn = (Turn + 1) % Strategies.Count;
        }
    }

    [XmlInclude(typeof(WallMove))]
    [XmlInclude(typeof(PlayerMove))]
    public abstract class MoveInfo
    {
        public abstract Movement GetMovement();
    }

    [Serializable]
    public class WallMove : MoveInfo
    {
        [XmlAttribute(nameof(X))]
        public int X { get; set; }

        [XmlAttribute(nameof(Y))]
        public int Y { get; set; }

        [XmlAttribute(nameof(Placement))]
        public string _placement { get; set; }

        [XmlIgnore]
        public Direction Placement => EnumHelper.ParseEnum<Direction>(_placement);

        public override Movement GetMovement()
        {
            return new Wall(Placement, new(X, Y));
        }
    }

    [Serializable]
    public class PlayerMove : MoveInfo
    {
        [XmlAttribute(nameof(X))]
        public int X { get; set; }

        [XmlAttribute(nameof(Y))]
        public int Y { get; set; }

        public override Movement GetMovement()
        {
            return new Vector2(X, Y);
        }
    }

    [XmlInclude(typeof(HumanStrategy))]
    [XmlInclude(typeof(AStarStrategy))]
    [XmlInclude(typeof(RandomStrategy))]
    [XmlInclude(typeof(MctsStrategy))]
    [XmlInclude(typeof(MinimaxStrategy))]
    [XmlInclude(typeof(MinimaxABStrategy))]
    [XmlInclude(typeof(ParallelMinimaxABStrategy))]
    public abstract class Strategy
    {
        [XmlAttribute(nameof(Description))]
        public string Description { get; set; }

        public abstract IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy();
    }

    [Serializable]
    public class HumanStrategy : Strategy
    {
        public override IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class AStarStrategy : Strategy
    {
        [XmlIgnore]
        private AStar<Movement, IGameEnvironment, IPlayer> _astar;

        public override IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy()
        {
            return _astar ??= new();
        }
    }

    [Serializable]
    public class RandomStrategy : Strategy
    {

        [XmlAttribute(nameof(Seed))]
        public int Seed { get; set; } = 42;

        [XmlIgnore]
        private RandomStrategy<Movement, IGameEnvironment, IPlayer> _randomStrategy;

        public override IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy()
        {
            return _randomStrategy ??= new(Seed);
        }
    }

    [Serializable]
    public class MctsStrategy : Strategy
    {
        [XmlAttribute(nameof(C))]
        public double C { get; set; } = 1.41;

        [XmlAttribute(nameof(Simulations))]
        public int Simulations { get; set; } = 1000;

        [XmlElement(nameof(Strategy))]
        public Strategy MoveStrategy { get; set; }

        [XmlIgnore]
        private MonteCarloTreeSearch<Movement, IGameEnvironment, IPlayer> _mctsStrategy;

        [XmlIgnore]
        private UCT<Movement, IPlayer, IGameEnvironment> _uctSelection;

        public override IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy()
        {
            if (_uctSelection == null) _uctSelection = new(C);
            return _mctsStrategy ??= new(Simulations, _uctSelection, MoveStrategy.GetStrategy());
        }
    }

    [Serializable]
    public class MinimaxStrategy : Strategy
    {
        [XmlAttribute(nameof(Depth))]
        public int Depth { get; set; } = 2;

        [XmlIgnore]
        private Minimax<IPlayer, Movement, IGameEnvironment> _minimax;

        public override IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy()
        {
            return _minimax ??= new(Depth);
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
