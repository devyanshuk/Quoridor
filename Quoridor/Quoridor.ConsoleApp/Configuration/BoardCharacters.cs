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

        [XmlElement(nameof(CellProperties))]
        public CellProperties CellProperty { get; set; }
    }

    [Serializable]
    public class CellProperties
    {
        [XmlElement(nameof(CellWidth))]
        public int _cellWidth { get; set; }

        [XmlIgnore]
        public int CellWidth
        {
            get
            {
                if (_cellWidth % 2 == 0)
                    throw new Exception($"Cell width must be an odd integer");
                return _cellWidth;
            }
        }

        [XmlIgnore]
        public int HalfCellWidth => CellWidth / 2;
    }

    [Serializable]
    public class BorderSeparators
    {
        [XmlElement(nameof(HorizontalBorderSeparator))]
        public string HorizontalBorderSeparator { get; set; }

        [XmlElement(nameof(VerticalBorderSeparator))]
        public string VerticalBorderSeparator { get; set; }

        [XmlElement(nameof(IntersectionBorderSeparator))]
        public string IntersectionBorderSeparator { get; set; }
    }

    [Serializable]
    public class WallSeparators
    {
        [XmlElement(nameof(HorizontalWallSeparator))]
        public string HorizontalWallSeparator { get; set; }

        [XmlElement(nameof(VerticalWallSeparator))]
        public string VerticalWallSeparator { get; set; }
    }
}
