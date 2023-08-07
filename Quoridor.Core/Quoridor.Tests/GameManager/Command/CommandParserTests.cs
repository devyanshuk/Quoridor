﻿using System;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;

using Quoridor.Core.Game;
using Quoridor.Core.Utils;
using static Quoridor.Core.Utils.Direction;
using Quoridor.ConsoleApp.GameManager.Command;

namespace Quoridor.Tests.GameManager.Command
{
    [TestFixture]
    public class CommandParserTests
    {
        [TestCase("Place wall (5,5) south", 5, 5, South)]
        [TestCase("   place WAlL at south at location (3, 4)", 3, 4, South)]
        [TestCase("pLACE         wall (5,    3) at north please", 5, 3, North)]
        [TestCase("wall 4, 2 easT", 4, 2, East)]
        [TestCase("could you please place a northern wall at coordinate (2,1)?", 2, 1, North)]
        public void Should_Correctly_Parse_Valid_Wall_Commands(
            string line, int w_x, int w_y, Direction dir)
        {
            //Arrange
            var commandParser = GetCommandParser();

            //Act
            var wallCommand = commandParser.Parse(line);

            //Assert
            wallCommand.Should().BeOfType<WallCommand>();
            var wall = wallCommand as WallCommand;
            wall.Dir.Should().Be(dir);
            wall.Pos.X.Should().Be(w_x);
            wall.Pos.Y.Should().Be(w_y);
        }

        [TestCase("Move North", North)]
        [TestCase("Move         North", North)]
        [TestCase("move     SoUtH", South)]
        [TestCase("hey, could you please move up in northern side?", North)]
        [TestCase("EAST move pls", East)]
        [TestCase("        move west       ", West)]
        [TestCase("west move ,,,,,,,,,,", West)]
        public void Should_Correctly_Parse_Valid_Move_Commands(
            string line, Direction dir)
        {
            //Arrange
            var commandParser = GetCommandParser();

            //Act
            var moveCommand = commandParser.Parse(line);

            //Assert
            moveCommand.Should().BeOfType<MoveCommand>();
            var move = moveCommand as MoveCommand;
            move.Dir.Should().Be(dir);
        }

        [TestCase("move wall")]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("\n")]
        [TestCase("wall move")]
        [TestCase("some_random_text")]
        [TestCase("            ")]
        [TestCase("place wall south")]
        [TestCase("place wall (5,5)")]
        [TestCase("place wall 5, 5 n o r t h")]
        [TestCase("north place w a l l (2, 3)")]
        [TestCase("move")]
        [TestCase("move n o r t h")]
        public void Should_Throw_If_Invalid_Command_Is_Given(string line)
        {
            //Arrange
            var commandParser = GetCommandParser();

            //Act
            Action a = () => commandParser.Parse(line);

            //Assert
            a.Should().Throw<ArgumentException>();
        }

        public class RandomCommand : BaseCommand { }

        [Test]
        public void Should_Throw_If_Process_Command_Does_Not_Recognize_Command_Type()
        {
            //Arrange
            var commandParser = GetCommandParser();
            var randomCommand = new RandomCommand { Dir = South };

            //Act
            Action a = () => commandParser.Process(randomCommand);

            //Assert
            a.Should().Throw<ArgumentException>();
        }


        private CommandParser GetCommandParser()
        {
            return new CommandParser(null, null, Substitute.For<IGameEnvironment>());
        }
    }

}