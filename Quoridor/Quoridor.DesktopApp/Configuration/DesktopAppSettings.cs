using System;
using System.Drawing;
using System.Xml.Serialization;
using System.Collections.Generic;

using Quoridor.AI;
using Quoridor.Core;
using Quoridor.Core.Utils;
using Quoridor.Common.Helpers;
using Quoridor.Core.Environment;

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

        public int WallHeight
        {
            get
            {
                return CellSize * 2 + FormSettings.WallWidth;
            }
        }
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
        [XmlArrayItem(nameof(Strategy), typeof(Strategy))]
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

    [Serializable]
    public class Strategy
    {
        [XmlAttribute(nameof(Name))]
        public string _name { get; set; } = AITypes.Human.ToString();

        [XmlAttribute(nameof(Depth))]
        public int Depth { get; set; } = 2;

        [XmlAttribute(nameof(C))]
        public double C { get; set; } = 1.41;

        [XmlAttribute(nameof(Simulations))]
        public int Simulations { get; set; } = 1000;

        [XmlIgnore]
        public AITypes Name => EnumHelper.ParseEnum<AITypes>(_name);
    }
}
