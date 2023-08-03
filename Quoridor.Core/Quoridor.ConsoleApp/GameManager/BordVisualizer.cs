using System;
using System.IO;
using System.Text;

using Quoridor.Common.Logging;
using Quoridor.ConsoleApp.Configuration;
using Quoridor.Core.Environment;

namespace Quoridor.ConsoleApp.GameManager
{
    public class BoardVisualizer : IBoardVisualizer
    {
        private readonly ILogger _log = Logger.InstanceFor<BoardVisualizer>();

        private readonly char _playerALabel;
        private readonly char _playerBLabel;
        private readonly TextWriter _stdOut;
        private readonly IBoard _board;
        private readonly IConfigProvider _configProvider;

        public BoardVisualizer(
            char playerALabel,
            char playerBLabel,
            TextWriter stdOut,
            IBoard board,
            IConfigProvider configProvider
        )
        {
            _playerALabel = playerALabel;
            _playerBLabel = playerBLabel;
            _stdOut = stdOut;
            _board = board;
            _configProvider = configProvider;
        }

        public void DrawBoard()
        {
            var sb = new StringBuilder();
            var a = _configProvider.BoardChars;


            _stdOut.WriteLine(sb);
        }
    }
}
