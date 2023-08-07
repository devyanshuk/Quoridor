using System;
using System.IO;
using System.Text.RegularExpressions;

using Quoridor.Core.Game;
using Quoridor.Core.Utils;
using System.Globalization;
using Quoridor.Common.Logging;

namespace Quoridor.ConsoleApp.GameManager.Command
{
    public class CommandParser : ICommandParser
    {
        private readonly TextReader _stdIn;
        private readonly TextWriter _stdOut;
        private readonly IGameEnvironment _gameEnvironment;

        private readonly ILogger _log = Logger.InstanceFor<CommandParser>();

        private readonly Regex dirRegex = new Regex(
            $"(?<{nameof(Direction)}>[Nn][Oo][Rr][Tt][Hh]|[Ss][Oo][Uu][Tt][Hh]|[Ee][Aa][Ss][Tt]|[Ww][Ee][Ss][Tt])");
        private readonly Regex moveRegex = new Regex($"(?<{nameof(MoveType)}>[Mm][Oo][Vv][Ee])");
        private readonly Regex wallRegex = new Regex($"(?<{nameof(MoveType)}>[Ww][Aa][Ll][Ll])");
        private readonly Regex coordinateRegex = new Regex("(?<X>\\d+)\\s*,?\\s*(?<Y>\\d+)");

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
            var player = _gameEnvironment.CurrentPlayer;

            _stdOut.WriteLine($"Player '{player.Id}''s Turn. {player.NumWalls} wall(s) left");

            while (true)
            {
                var line = _stdIn.ReadLine();
                try
                {
                    var command = Parse(line);
                    Process(command);
                    break;
                }
                catch(Exception ex)
                {
                    _stdOut.WriteLine(ex.Message);
                }
            }
            _gameEnvironment.ChangeTurn();
        }

        public BaseCommand Parse(string line)
        {
            _log.Info($"Processing line : '{line}'...");

            var dir = dirRegex.Match(line);
            var move = moveRegex.Match(line);
            var wall = wallRegex.Match(line);
            var coordinate = coordinateRegex.Match(line);

            // !dir V move iff wall V wall iff !coordinate
            if ((!dir.Success) ||
                (!move.Success && !wall.Success) ||
                (move.Success && wall.Success) ||
                (wall.Success && !coordinate.Success) ||
                (coordinate.Success && !wall.Success))
            {
                var message = $"'{line}' : Invalid command";
                _log.Error(message);
                throw new ArgumentException(message);
            }

            var directionStr = dir.Groups[nameof(Direction)].Value.ToLower();
            var dirEnum = ParseEnum<Direction>(
                CultureInfo.CurrentCulture.TextInfo.ToTitleCase(directionStr));

            _log.Info($"Parsed direction '{dirEnum}' from '{line}'");

            if (dir.Success && move.Success)
                return new MoveCommand { Dir = dirEnum };

            var x = int.Parse(coordinate.Groups["X"].Value);
            var y = int.Parse(coordinate.Groups["Y"].Value);
            var pos = new Vector2(x, y);
            _log.Info($"Parsed coordinate '{pos}' from '{line}'");

            return new WallCommand { Dir = dirEnum, Pos = pos };
        }

        public void Process<T>(T command) where T : BaseCommand
        {
            if (command is MoveCommand moveCommand)
            {
                _log.Info($"Move command entered. Moving player '{_gameEnvironment.Turn}'");
                _gameEnvironment.MovePlayer(moveCommand.Dir);
            }
            else if (command is WallCommand wallCommand)
            {
                _gameEnvironment.AddWall(wallCommand.Pos, wallCommand.Dir);
                _log.Info($"Successfully added wall '{wallCommand.Pos}': '{wallCommand.Dir}'");
            }
            else throw new ArgumentException($"{typeof(T).Name} command type not recognized");
        }

        private T ParseEnum<T>(string val)
        {
            return (T)Enum.Parse(typeof(T), val);
        }

    }
}
