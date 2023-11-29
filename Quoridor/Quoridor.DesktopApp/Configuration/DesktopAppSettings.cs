using System;
using System.Drawing;
using System.Xml.Serialization;

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
    }

    [Serializable]
    public class FormSettings
    {
        [XmlElement(nameof(Title))]
        public string Title { get; set; }

        [XmlElement(nameof(ScreenWidth))]
        public int _screenWidth { get; set; }

        [XmlElement(nameof(ScreenHeight))]
        public int _screenHeight { get; set; }

        [XmlElement(nameof(OffsetX))]
        public int OffsetX { get; set; }

        [XmlElement(nameof(OffsetY))]
        public int OffsetY { get; set; }

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
        [XmlElement(nameof(WallWidth))]
        public int WallWidth { get; set; }
    }
}
