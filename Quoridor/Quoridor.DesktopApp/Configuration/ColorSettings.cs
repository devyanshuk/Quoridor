using System;
using System.Drawing;
using System.Xml.Serialization;

using Quoridor.Core.Utils;

namespace Quoridor.DesktopApp.Configuration
{
    [Serializable]
    public class ColorSettings
    {
        public const int MAX_OPACITY = 255;

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

        [XmlElement(nameof(CurrentPlayerCellColor))]
        public string _currentPlayerCellColor { get; set; }

        [XmlElement(nameof(PossibleCellMoveColor))]
        public string _possibleCellMoveColor { get; set; }

        [XmlElement(nameof(PossibleWallColor))]
        public string _possibleWallColor { get; set; }

        [XmlElement(nameof(WallColor))]
        public string _wallColor { get; set; }

        [XmlElement(nameof(Opacity))]
        public int Opacity { get; set; }

        [XmlIgnore]
        public int AnimatableOpacity { get; set; } = 0;

        [XmlIgnore]
        public Direction OpacityUpdateDirection { get; set; } = Direction.North;

        [XmlElement(nameof(OpacityAnimationVelocity))]
        public int OpacityAnimationVelocity { get; set; }

        [XmlIgnore]
        public Color PossibleWallColor => Color.FromArgb(AnimatableOpacity, Color.FromName(_possibleWallColor));

        [XmlIgnore]
        public Color PossibleCellMoveColor => Color.FromArgb(AnimatableOpacity, Color.FromName(_possibleCellMoveColor));

        [XmlIgnore]
        public Color CurrentPlayerCellColor => Color.FromName(_currentPlayerCellColor);

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

        public void UpdateOpacity()
        {
            if (OpacityUpdateDirection.Equals(Direction.North))
            {
                AnimatableOpacity += OpacityAnimationVelocity;
                if (AnimatableOpacity >= MAX_OPACITY)
                {
                    OpacityUpdateDirection = Direction.South;
                    AnimatableOpacity = MAX_OPACITY;
                }
            }
            else
            {
                AnimatableOpacity -= OpacityAnimationVelocity;
                if (AnimatableOpacity <= 0)
                {
                    OpacityUpdateDirection = Direction.North;
                    AnimatableOpacity = 0;
                }
            }
        }

    }
}
