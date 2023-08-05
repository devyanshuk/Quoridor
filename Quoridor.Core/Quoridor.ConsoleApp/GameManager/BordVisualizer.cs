using System;
using System.IO;
using System.Linq;
using System.Text;

using Quoridor.Common.Logging;
using Quoridor.ConsoleApp.Configuration;
using Quoridor.Core.Environment;
using Quoridor.Core.Game;

namespace Quoridor.ConsoleApp.GameManager
{
    public class BoardVisualizer : IBoardVisualizer
    {
        private readonly ILogger _log = Logger.InstanceFor<BoardVisualizer>();

        private readonly TextWriter _stdOut;
        private readonly IBoard _board;
        private readonly IConfigProvider _configProvider;
        private readonly IGameEnvironment _gameEnvironment;

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
        }

        public void DrawBoard()
        {
            var sb = new StringBuilder();

            AppendInitialRow(sb);
            for (var i = 0; i < _board.Dimension; i++)
            {
                AppendWallRow(sb);

                for (var j = 0; j < _configProvider.BoardChars.CellProperty.CellWidth / 2; j++)
                    AppendCellRow(sb, i, j == 0);
            }
            AppendWallRow(sb);

            _stdOut.WriteLine(sb);

        }

        private void AppendWallRow(StringBuilder sb)
        {
            for (var k = 0; k < _board.Dimension + 1; k++)
            {
                sb.Append(PadStrWithChar(
                    _configProvider.BoardChars.BorderSeparator.Horizontal));

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
            sb.Append( PadStr(displayItems ? rowNumber.ToString() : " ") );

            sb.Append(_configProvider.BoardChars.BorderSeparator.Vertical);

            for (var j = 0; j < _board.Dimension; j++)
            {
                var cell = " ";

                if (displayItems)
                {
                    var player = _gameEnvironment.Players.FirstOrDefault(
                        p => p.CurrentPos.X.Equals(rowNumber) && p.CurrentPos.Y.Equals(j));

                    if (player != null)
                        cell = player.Id.ToString();
                }

                sb.Append(PadStr(cell));
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
