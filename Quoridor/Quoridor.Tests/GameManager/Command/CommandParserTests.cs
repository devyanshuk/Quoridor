using System;
using NUnit.Framework;
using FluentAssertions;

using Quoridor.Core.Game;
using Quoridor.Core.Utils;
using Quoridor.Core.Environment;
using static Quoridor.Core.Utils.Direction;
using Quoridor.ConsoleApp.GameManager.Command;

namespace Quoridor.Tests.GameManager.Command
{
    [TestFixture]
    public class CommandParserTests
    {
        [TestCase("wall 4, 2 easT", 4, 2, East)]
        [TestCase("northern wall at (5,5)", 5, 5, North)]
        [TestCase("Place wall (5,5) south", 5, 5, South)]
        [TestCase("   place WAlL at south at location (3, 4)", 3, 4, South)]
        [TestCase("pLACE         wall (5,    3) at north please", 5, 3, North)]
        [TestCase("wall in the southern side at coordinate (5,5)", 5, 5, South)]
        [TestCase("could you please place a northern wall at coordinate (2,1)?", 2, 1, North)]
        public void Should_Correctly_Parse_Valid_Wall_Commands(
            string line, int w_x, int w_y, Direction dir)
        {
            //Arrange
            var commandParser = GetCommandParser();

            //Act
            var wallCommand = commandParser.Parse(line);

            //Assert
            wallCommand.Should().BeOfType<Wall>();
            var wall = wallCommand as Wall;
            wall.Placement.Should().Be(dir);
            wall.From.X.Should().Be(w_x);
            wall.From.Y.Should().Be(w_y);
        }

        [TestCase("Move North", North)]
        [TestCase("EAST move pls", East)]
        [TestCase("move     SoUtH", South)]
        [TestCase("Move         North", North)]
        [TestCase("west move ,,,,,,,,,,", West)]
        [TestCase("        move west       ", West)]
        [TestCase("hey, could you please move up in northern side?", North)]
        public void Should_Correctly_Parse_Valid_Move_Commands(
            string line, Direction dir)
        {
            //Arrange
            var commandParser = GetCommandParser();

            //Act
            var moveCommand = commandParser.Parse(line);

            //Assert
            moveCommand.Should().BeOfType<AgentMove>();
            var move = moveCommand as AgentMove;
            move.Dir.Should().Be(dir);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("\n")]
        [TestCase("move")]
        [TestCase("wall move")]
        [TestCase("move wall")]
        [TestCase("            ")]
        [TestCase("move n o r t h")]
        [TestCase("some_random_text")]
        [TestCase("place wall south")]
        [TestCase("place wall (5,5)")]
        [TestCase("place wall 5, 5 n o r t h")]
        [TestCase("north place w a l l (2, 3)")]
        public void Should_Throw_If_Invalid_Command_Is_Given(string line)
        {
            //Arrange
            var commandParser = GetCommandParser();

            //Act
            Action a = () => commandParser.Parse(line);

            //Assert
            a.Should().Throw<ArgumentException>();
        }

        private CommandParser GetCommandParser()
        {
            return new CommandParser();
        }
    }

}
