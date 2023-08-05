using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Quoridor.Common.Logging;
using Quoridor.ConsoleApp.Configuration;
using Quoridor.Core.Environment;
using Quoridor.Core.Game;
using Quoridor.Core.Utils;

namespace Quoridor.ConsoleApp.GameManager
{
    public class BoardVisualizer : IBoardVisualizer
    {
        private readonly ILogger _log = Logger.InstanceFor<BoardVisualizer>();

        private readonly TextWriter _stdOut;
        private readonly IBoard _board;
        private readonly IConfigProvider _configProvider;
        private readonly IGameEnvironment _gameEnvironment;

        private int[] _verticalWalls;

        public BoardVisualizer(
            TextWriter stdOut,
            IBoard board,
            IConfigProvider configProvider,
            IGameEnvironment gameEnvironment
        )
        {
            _stdOut = stdOut;
            _board = board;
            _configProvider = configProvider;
            _gameEnvironment = gameEnvironment;

            _verticalWalls = new int[_board.Dimension];

            _gameEnvironment.AddWall(new Vector2(5, 5), Direction.South);
            _gameEnvironment.AddWall(new Vector2(5, 5), Direction.East);
        }

        public void DrawBoard()
        {
            var sb = new StringBuilder();

            AppendInitialRow(sb);
            for (var i = 0; i < _board.Dimension; i++)
            {
                AppendWallRow(sb, i);

                for (var j = 0; j < _configProvider.BoardChars.CellProperty.CellWidth / 2; j++)
                    AppendCellRow(sb, i, j == 0);
            }
            AppendWallRow(sb, _board.Dimension);

            _stdOut.WriteLine(sb);
        }

        private void AppendWallRow(StringBuilder sb, int row)
        {
            var wallCount = 0;

            sb.Append(PadStrWithChar(_configProvider.BoardChars.BorderSeparator.Horizontal));
            sb.Append(_configProvider.BoardChars.BorderSeparator.Intersection);

            for (var k = 0; k < _board.Dimension; k++)
            {
                if (row < _board.Dimension - 1 && !_board.GetCell(k, row).IsAccessible(Direction.South))
                {
                    sb.Append(PadStrWithChar(_configProvider.BoardChars.WallSeparator.Horizontal));
                    wallCount++;
                }
                else
                    sb.Append(PadStrWithChar(_configProvider.BoardChars.BorderSeparator.Horizontal));

                if (_verticalWalls[k] % 2 == 1 || wallCount % 2 == 1)
                    sb.Append(_configProvider.BoardChars.WallSeparator.Horizontal);
                else
                    sb.Append(_configProvider.BoardChars.BorderSeparator.Intersection);
            }
            sb.AppendLine();
        }

        private void AppendInitialRow(StringBuilder sb)
        {
            sb.Append(PadStr(" "));
            for (var i = 0; i < _board.Dimension; i++)
            {
                sb.Append(_configProvider.BoardChars.BorderSeparator.Vertical);
                sb.Append(PadStr(i));
            }
            sb.AppendLine(_configProvider.BoardChars.BorderSeparator.Vertical);
        }

        private void AppendCellRow(StringBuilder sb, int rowNumber, bool displayItems)
        {
            sb.Append(PadStr(displayItems ? rowNumber.ToString() : " "));
            sb.Append(_configProvider.BoardChars.BorderSeparator.Vertical);

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
                    sb.Append(_configProvider.BoardChars.WallSeparator.Vertical);
                    if (displayItems)
                        _verticalWalls[j]++;
                }
                else
                    sb.Append(_configProvider.BoardChars.BorderSeparator.Vertical);
            }
            sb.AppendLine();
        }

        private string PadStr<T>(T item)
        {
            var padLen = _configProvider.BoardChars.CellProperty.HalfCellWidth;

            var itemStr = item.ToString();
            var right = itemStr.PadRight(padLen + itemStr.Length);
            var left = right.PadLeft(padLen * 2 + itemStr.Length);
            return left;
        }

        private string PadStrWithChar<T>(T item)
        {
            var padLen = _configProvider.BoardChars.CellProperty.HalfCellWidth;

            var itemStr = item.ToString();
            var padChar = itemStr[0];
            var right = itemStr.PadRight(padLen + itemStr.Length, padChar);
            var left = right.PadLeft(padLen * 2 + itemStr.Length, padChar);
            return left;
        }
    }
}
