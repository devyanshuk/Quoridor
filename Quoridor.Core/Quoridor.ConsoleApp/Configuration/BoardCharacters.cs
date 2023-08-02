using System;
using System.Xml.Serialization;

namespace Quoridor.ConsoleApp.Configuration
{
    [XmlRoot(nameof(BoardChars))]
    public class BoardChars
    {
        [XmlElement(nameof(BorderSeparators))]
        public BorderSeparators BorderSeparator { get; set; }

        [XmlElement(nameof(WallSeparators))]
        public WallSeparators WallSeparator { get; set; }
        
    }

    [Serializable]
    public class BorderSeparators
    {
        [XmlElement("HorizontalSeparator")]
        public string Horizontal { get; set; }

        [XmlElement("VerticalSeparator")]
        public string Vertical { get; set; }

        [XmlElement("IntersectionSeparator")]
        public string Intersection { get; set; }
    }

    [Serializable]
    public class WallSeparators
    {
        [XmlElement("HorizontalWallSeparator")]
        public string Horizontal { get; set; }

        [XmlElement("VerticalWallSeparator")]
        public string Vertical { get; set; }
    }
}
