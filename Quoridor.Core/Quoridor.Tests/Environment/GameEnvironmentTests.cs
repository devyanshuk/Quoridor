using System;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

using Quoridor.Core.Utils;
using Quoridor.Core.Environment;
using Quoridor.Core.Utils.CustomExceptions;
using static Quoridor.Core.Utils.Direction;

namespace Quoridor.Tests.Environment
{
    [TestFixture]
    public class GameEnvironmentTests
    {
        [TestCase(South, -1, 0)]
        [TestCase(North, 100, 100)]
        [TestCase(East, 0, 9)]
        public void Should_Throw_If_Incorrect_Wall_Is_Placed_Outside_Of_Board_Bounds(
            Direction dir, int f_x, int f_y)
        {
            //Arrange
            var gameEnv = CreateGameEnvironment().Item2;

            //Act
            Action a = () => gameEnv.AddWall(new Vector2(f_x, f_y), dir);

            //Assert
            a.Should().Throw<InvalidWallException>();
        }

        [TestCase(0, 0, North)]
        [TestCase(0, 8, West)]
        [TestCase(0, 1, West)]
        [TestCase(8, 0, East)]
        [TestCase(8, 0, North)]
        [TestCase(8, 8, South)]
        [TestCase(0, 8, South)]
        [TestCase(0, 8, West)]
        public void Should_Throw_If_Incorrect_Wall_Is_Placed_In_Board_Corners(
            int f_x, int f_y, Direction placement)
        {
            //Arrange
            var gameEnv = CreateGameEnvironment().Item2;

            //Act
            Action a = () => gameEnv.AddWall(new Vector2(f_x, f_y), placement);

            //Assert
            a.Should().Throw<InvalidWallException>();
            
        }

        [TestCase(5, 5, North, 6, 5)]
        [TestCase(5, 5, South, 6, 5)]
        [TestCase(3, 4, East, 3, 4)]
        [TestCase(5, 5, West, 5, 6)]
        public void Should_Throw_If_Wall_Added_WRT_Another_Cell_Is_Already_Present_Or_Intersects_With_Another_Wall(
            int f_x, int f_y, Direction dir, int t_x, int t_y)
        {
            //Arrange
            var gameEnv = CreateGameEnvironment().Item2;
            var pos = new Vector2(f_x, f_y);
            var pos2 = new Vector2(t_x, t_y);

            //Act
            gameEnv.AddWall(pos, dir);
            Action a = () => gameEnv.AddWall(pos2, dir);
            Action b = () => gameEnv.AddWall(pos, dir);

            //Assert
            a.Should().Throw<WallAlreadyPresentException>();
            b.Should().Throw<WallAlreadyPresentException>();
        }

        [TestCase(5, 5, North)]
        [TestCase(5, 5, South)]
        [TestCase(3, 4, East)]
        public void Should_Correctly_Set_Walls_If_Added_Correctly(
            int f_x, int f_y, Direction dir)
        {
            //Arrange
            var token = CreateGameEnvironment();
            var gameEnv = token.Item2;
            var board = token.Item1;
            var pos = new Vector2(f_x, f_y);

            //Act
            gameEnv.AddWall(pos, dir);

            //Assert
            board.GetCell(pos).IsAccessible(dir).Should().Be(false);
        }

        [Test]
        public void Should_Correctly_Set_Multiple_Walls_Arround_A_Cell()
        {
            //Arrange
            var items = CreateGameEnvironment();
            var board = items.Item1;
            var gameEnv = items.Item2;
            var pos = new Vector2(5, 5);
            var pos2 = new Vector2(5, 6);
            var pos3 = new Vector2(4, 6);
            var pos4 = new Vector2(6, 5);

            //Act
            gameEnv.AddWall(pos, North);
            gameEnv.AddWall(pos, South);
            gameEnv.AddWall(pos, West);

            //Assert
            //(5,5) is blocked on the northern, southern and western sides
            board.GetCell(pos).IsAccessible(North).Should().Be(false);
            board.GetCell(pos).IsAccessible(South).Should().Be(false);
            board.GetCell(pos).IsAccessible(West).Should().Be(false);
            board.Neighbors(board.GetCell(pos)).Count().Should().Be(1);

            //(5,6) is blocked on the northern and the western sides
            board.GetCell(pos2).IsAccessible(North).Should().Be(false);
            board.GetCell(pos2).IsAccessible(West).Should().Be(false);
            board.Neighbors(board.GetCell(pos2)).Count().Should().Be(2);

            //(6,5) is blocked on the northern and southen sides
            board.GetCell(pos4).IsAccessible(North).Should().Be(false);
            board.GetCell(pos4).IsAccessible(South).Should().Be(false);
            board.Neighbors(board.GetCell(pos4)).Count().Should().Be(2);

            //(4,6) is blocked on the eastern side
            board.GetCell(pos3).IsAccessible(East).Should().Be(false);
            board.Neighbors(board.GetCell(pos3)).Count().Should().Be(3);
        }

        [TestCase(5, 5, North)]
        [TestCase(7, 5, South)]
        public void Should_Create_And_Validate_Walls(
            int f_x, int f_y, Direction dir)
        {
            //Arrange
            var gameEnvironment = CreateGameEnvironment().Item2;
            var from = new Vector2(f_x, f_y);

            //Act
            var wall = gameEnvironment.CreateAndValidateWall(from, dir);

            //Assert
            wall.From.Should().Be(from);
            wall.Placement.Should().Be(dir);
        }

        [TestCase(0, 0, North)]
        [TestCase(0, 8, South)]
        [TestCase(8, 0, East)]
        [TestCase(2, 8, South)]
        public void Should_Throw_If_Wall_Could_Not_Be_Validated(
            int f_x, int f_y, Direction dir)
        {
            //Arrange
            var gameEnvironment = CreateGameEnvironment().Item2;
            var pos = new Vector2(f_x, f_y);

            //Act
            Action a = () => gameEnvironment.CreateAndValidateWall(pos, dir);

            //Assert
            a.Should().Throw<Exception>();
        }

        private Tuple<IBoard, GameEnvironment> CreateGameEnvironment()
        {
            var board = new Board();
            board.SetDimension(9);
            return Tuple.Create<IBoard, GameEnvironment>(board, new GameEnvironment(board));
        }
    }
}
