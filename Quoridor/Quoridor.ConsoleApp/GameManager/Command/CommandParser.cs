using System;
using System.Globalization;
using System.Text.RegularExpressions;

using Quoridor.Core;
using Quoridor.Core.Game;
using Quoridor.Core.Utils;
using Quoridor.Common.Logging;
using Quoridor.Common.Helpers;
using Quoridor.Core.Environment;

namespace Quoridor.ConsoleApp.GameManager.Command
{
    public class CommandParser : ICommandParser
    {
        private readonly ILogger _log = Logger.InstanceFor<CommandParser>();

        private readonly Regex dirRegex = new Regex(
            $"(?<{nameof(Direction)}>[Nn][Oo][Rr][Tt][Hh]|[Ss][Oo][Uu][Tt][Hh]|[Ee][Aa][Ss][Tt]|[Ww][Ee][Ss][Tt])");
        private readonly Regex moveRegex = new Regex($"(?<{nameof(MoveType)}>[Mm][Oo][Vv][Ee])");
        private readonly Regex wallRegex = new Regex($"(?<{nameof(MoveType)}>[Ww][Aa][Ll][Ll])");
        private readonly Regex coordinateRegex = new Regex("(?<X>\\d+)\\s*,?\\s*(?<Y>\\d+)");

        public CommandParser()
        {
        }

        public Movement Parse(string line)
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
            var dirEnum = EnumHelper.ParseEnum<Direction>(
                CultureInfo.CurrentCulture.TextInfo.ToTitleCase(directionStr));

            _log.Info($"Parsed direction '{dirEnum}' from '{line}'");

            if (dir.Success && move.Success)
                return new AgentMove(dirEnum, null);

            var x = int.Parse(coordinate.Groups["X"].Value);
            var y = int.Parse(coordinate.Groups["Y"].Value);
            var pos = new Vector2(x, y);
            _log.Info($"Parsed coordinate '{pos}' from '{line}'");

            return new Wall(dirEnum, pos);
        }
    }
}
