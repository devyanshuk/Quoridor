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
    }

    [Serializable]
    public class FormSettings
    {
        [XmlElement(nameof(Title))]
        public string Title { get; set; }

        [XmlElement(nameof(ScreenHeight))]
        public int ScreenHeight { get; set; }

        [XmlElement(nameof(ScreenWidth))]
        public int ScreenWidth { get; set; }
    }

    [Serializable]
    public class ColorSettings
    {
        [XmlElement(nameof(OddTileColor))]
        public string _oddTileColor { get; set; }

        [XmlElement(nameof(EvenTileColor))]
        public string _evenTileColor { get; set; }

        public Color OddTileColor => Color.FromName(_oddTileColor);

        public Color EvenTileColor => Color.FromName(_evenTileColor);
    }
}
