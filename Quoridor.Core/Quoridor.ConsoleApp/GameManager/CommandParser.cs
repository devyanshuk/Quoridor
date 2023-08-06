using System;
using System.IO;
using System.Text.RegularExpressions;
using Quoridor.Common.Logging;
using Quoridor.Core.Entities;
using Quoridor.Core.Game;
using Quoridor.Core.Utils;

namespace Quoridor.ConsoleApp.GameManager
{
    public class CommandParser : ICommandParser
    {
        private readonly TextReader _stdIn;
        private readonly TextWriter _stdOut;
        private readonly IGameEnvironment _gameEnvironment;

        private readonly ILogger _log = Logger.InstanceFor<CommandParser>();

        private readonly Regex commandPattern = new Regex("^\\s*(?<Type>Move|Wall\\s*\\(?\\s*(?<X>\\d)\\s*,?\\s*(?<Y>\\d))\\s*\\)?\\s*(?<Dir>North|South|East|West)");

        public CommandParser(
            TextReader stdIn,
            TextWriter stdOut,
            IGameEnvironment gameEnvironment
            )
        {
            _stdIn = stdIn;
            _stdOut = stdOut;
            _gameEnvironment = gameEnvironment;
        }

        public void ParseAndProcess()
        {
            var success = false;
            while (!success)
            {
                try
                {
                    success = Process();
                }
                catch(Exception ex)
                {
                    _stdOut.WriteLine(ex.Message);
                }
            }
            _gameEnvironment.ChangeTurn();
        }

        private bool Process()
        {
            var player = _gameEnvironment.CurrentPlayer;

            _stdOut.WriteLine($"Player '{player.Id}' has {player.NumWalls} left");

            var line = _stdIn.ReadLine();
            _log.Info($"Processing line : '{line}'...");

            if (!commandPattern.IsMatch(line))
            {
                var message = $"'{line}' : Invalid command";
                _log.Warn(message);
                _stdOut.WriteLine(message);
                return false;
            }
            var groups = commandPattern.Match(line).Groups;
            var type = groups["Type"].Value;
            var dir = ParseEnum<Direction>(groups["Dir"].Value);

            if (type == "Move")
            {
                _log.Info($"Move command entered. Moving player '{_gameEnvironment.Turn}'");
                _gameEnvironment.MovePlayer(dir);
            }
            else
            {
                var x = int.Parse(groups["X"].Value);
                var y = int.Parse(groups["Y"].Value);
                var pos = new Vector2(x, y);
                _log.Info($"Trying to add wall '{pos}': '{dir}'");

                _gameEnvironment.AddWall(pos, dir);
                _log.Info($"Successfully added wall '{pos}': '{dir}'");
            }
            return true;
        }

        private T ParseEnum<T>(string val)
        {
            return (T)Enum.Parse(typeof(T), val);
        }

    }
}
