using System.Drawing;
using System.Xml.Serialization;

using Quoridor.Core.Utils;

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

        [XmlIgnore]
        public int CellSize
        {
            get
            {
                var totalWallWidth = FormSettings.WallWidth * (GameSettings.Dimension - 1);
                var totalCellSize = FormSettings.ScreenWidth - totalWallWidth;
                var cellSize = totalCellSize / GameSettings.Dimension;
                return cellSize;
            }
        }

        [XmlIgnore]
        public int WallHeight => CellSize * 2 + FormSettings.WallWidth;

        public bool WithinCellBounds(Point pos, out Vector2 cellPos)
        {
            cellPos = default;

            if (!FormSettings.WithinBoardBounds(pos))
                return false;

            var posY = pos.Y - FormSettings.OffsetY;
            var cellSize = CellSize;
            var wallWidth = FormSettings.WallWidth;
            var x = pos.X / (cellSize + wallWidth);
            var y = posY / (cellSize + wallWidth);

            var left = x * (cellSize + wallWidth);
            var right = left + cellSize;
            var up = y * (cellSize + wallWidth);
            var down = up + cellSize;

            if (!(pos.X >= left && pos.X <= right && posY >= up && posY <= down))
                return false;

            cellPos = new(x, y);
            return true;
        }
    }
}
