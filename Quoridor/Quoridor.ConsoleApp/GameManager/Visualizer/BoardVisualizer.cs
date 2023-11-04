using System.IO;
using System.Linq;
using System.Text;

using Quoridor.Core.Game;
using Quoridor.Core.Utils;
using Quoridor.Core.Environment;
using Quoridor.ConsoleApp.Configuration;

namespace Quoridor.ConsoleApp.GameManager.Visualizer
{
    public class BoardVisualizer : IBoardVisualizer
    {
        private readonly IBoard _board;
        private readonly IConfigProvider _configProvider;
        private readonly IGameEnvironment _gameEnvironment;

        private int[] _verticalWalls;

        public BoardVisualizer(
            IBoard board,
            IConfigProvider configProvider,
            IGameEnvironment gameEnvironment
        )
        {
            _board = board;
            _configProvider = configProvider;
            _gameEnvironment = gameEnvironment;
        }

        public void DrawBoard(TextWriter dest)
        {
            _verticalWalls = new int[_board.Dimension];
            var sb = new StringBuilder();

            AppendInitialRow(sb);
            for (var i = 0; i < _board.Dimension; i++)
            {
                AppendWallRow(sb, i);

                for (var j = 0; j < _configProvider.BoardChars.CellProperty.CellWidth / 2; j++)
                    AppendCellRow(sb, i, j == 0);
            }
            AppendWallRow(sb, _board.Dimension);

            dest.WriteLine(sb);
        }

        private void AppendWallRow(StringBuilder sb, int row)
        {
            var wallCount = 0;
            var boardChars = _configProvider.BoardChars;

            sb.Append(PadStrWithChar(boardChars.BorderSeparator.HorizontalBorderSeparator));
            sb.Append(boardChars.BorderSeparator.IntersectionBorderSeparator);

            for (var k = 0; k < _board.Dimension; k++)
            {
                if (row > 0 && row < _board.Dimension && !_board.GetCell(k, row).IsAccessible(Direction.North))
                {
                    sb.Append(PadStrWithChar(boardChars.WallSeparator.HorizontalWallSeparator));
                    wallCount++;
                }
                else
                    sb.Append(PadStrWithChar(boardChars.BorderSeparator.HorizontalBorderSeparator));

                if (_verticalWalls[k] % 2 == 1)
                    sb.Append(boardChars.WallSeparator.VerticalWallSeparator);
                else if (wallCount % 2 == 1)
                    sb.Append(boardChars.WallSeparator.HorizontalWallSeparator);
                else
                    sb.Append(boardChars.BorderSeparator.IntersectionBorderSeparator);
            }
            sb.AppendLine();
        }

        private void AppendInitialRow(StringBuilder sb)
        {
            sb.Append(PadStr(" "));
            for (var i = 0; i < _board.Dimension; i++)
            {
                sb.Append(_configProvider.BoardChars.BorderSeparator.VerticalBorderSeparator);
                sb.Append(PadStr(i));
            }
            sb.AppendLine(_configProvider.BoardChars.BorderSeparator.VerticalBorderSeparator);
        }

        /// <summary>
        /// Append either empty spaces, or cell numbers, player Ids.
        /// When displaying table, the user could configure cells to be bigger, so
        /// we use the 'displayItems' switch to only display the cell number, player Id (if any)
        /// in a row, and on the other, we append only empty spaces
        /// </summary>
        private void AppendCellRow(StringBuilder sb, int rowNumber, bool displayItems)
        {
            sb.Append(PadStr(displayItems ? rowNumber.ToString() : " "));
            sb.Append(_configProvider.BoardChars.BorderSeparator.VerticalBorderSeparator);

            for (var j = 0; j < _board.Dimension; j++)
            {
                var cellId = " ";
                var cell = _board.GetCell(j, rowNumber);

                if (displayItems)
                {
                    var player = _gameEnvironment.Players.FirstOrDefault(
                        p => p.CurrentPos.X.Equals(j) && p.CurrentPos.Y.Equals(rowNumber));

                    if (player != null)
                        cellId = player.Id.ToString();
                }
                sb.Append(PadStr(cellId));

                if (j < _board.Dimension - 1 && !cell.IsAccessible(Direction.East))
                {
                    sb.Append(_configProvider.BoardChars.WallSeparator.VerticalWallSeparator);
                    if (displayItems)
                        _verticalWalls[j]++;
                }
                else
                    sb.Append(_configProvider.BoardChars.BorderSeparator.VerticalBorderSeparator);
            }
            sb.AppendLine();
        }

        private string PadStr<T>(T item)
        {
            var padLen = _configProvider.BoardChars.CellProperty.HalfCellWidth;
            var itemStr = item.ToString();
            return itemStr.PadRight(padLen + itemStr.Length)
                .PadLeft(padLen * 2 + itemStr.Length);
        }

        private string PadStrWithChar<T>(T item)
        {
            var padLen = _configProvider.BoardChars.CellProperty.HalfCellWidth;

            var itemStr = item.ToString();
            var padChar = itemStr[0];
            return itemStr.PadRight(padLen + itemStr.Length, padChar)
                .PadLeft(padLen * 2 + itemStr.Length, padChar);
        }
    }
}
