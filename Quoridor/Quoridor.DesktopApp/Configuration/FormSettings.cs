using System;
using System.Drawing;
using System.Xml.Serialization;

namespace Quoridor.DesktopApp.Configuration
{

    [Serializable]
    public class FormSettings
    {
        [XmlIgnore]
        const int MIN_OFFSET_Y = 30;

        [XmlAttribute(nameof(Description))]
        public string Description { get; set; }

        [XmlElement(nameof(Title))]
        public string Title { get; set; }

        [XmlElement(nameof(ScreenWidth))]
        public int _screenWidth { get; set; }

        [XmlElement(nameof(ScreenHeight))]
        public int _screenHeight { get; set; }

        [XmlIgnore]
        private int _offsetY { get; set; } = MIN_OFFSET_Y;

        [XmlElement(nameof(OffsetY))]
        public int OffsetY
        {
            get { return _offsetY; }
            set
            {
                if (value < MIN_OFFSET_Y)
                    throw new Exception($"{nameof(OffsetY)} must be atleast {MIN_OFFSET_Y}");
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

        //board size (square) is the screen width
        public bool WithinBoardBounds(Point point)
        {
            return point.X >= 0 && point.X <= ScreenWidth && point.Y >= OffsetY && point.Y <= OffsetY + ScreenWidth;
        }
    }
}
