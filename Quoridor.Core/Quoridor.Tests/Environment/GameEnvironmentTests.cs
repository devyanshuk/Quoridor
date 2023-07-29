using System;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;

using Quoridor.Core.Utils;
using Quoridor.Core.Extensions;
using Quoridor.Core.Environment;
using Quoridor.Core.Utils.CustomExceptions;
using static Quoridor.Core.Utils.Direction;
using System.Linq;

namespace Quoridor.Tests.Environment
{
    [TestFixture]
    public class GameEnvironmentTests
    {
        [TestCase(South, -1, 0)]
        [TestCase(North, 100, 100)]
        [TestCase(East, 0, 9)]
        public void Should_Throw_If_Incorrect_Wall_Is_Placed_Outside_Of_Board_Bounds(
            Direction placement, int f_x, int f_y)
        {
            //Arrange
            var board = new Board();
            board.SetDimension(9);
            var from = new Vector2(f_x, f_y);
            var wallFactory = Substitute.For<IWallFactory>();

            var gameEnv = new GameEnvironment(board, wallFactory);

            //Act
            Action a = () => gameEnv.AddWall(from, placement);

            //Assert
            a.Should().Throw<InvalidWallException>();
        }

        [TestCase(0, 0, North, 0, -1)]
        [TestCase(0, 8, West, -1, 8)]
        [TestCase(0, 1, West, 0, 0)]
        [TestCase(8, 0, East, 9, 0)]
        [TestCase(8, 0, North, 9, 0)]
        [TestCase(8, 8, South, 9, 8)]
        [TestCase(0, 8, South, 0, 9)]
        [TestCase(0, 8, West, -1, 8)]
        public void Should_Throw_If_Incorrect_Wall_Is_Placed_In_Board_Corners(
            int f_x, int f_y, Direction placement, int t_x, int t_y)
        {
            //Arrange
            var gameEnv = CreateGameEnvironment(f_x, f_y, placement, t_x, t_y).Item2;

            //Act
            Action a = () => gameEnv.AddWall(new Vector2(f_x, f_y), placement);

            //Assert
            a.Should().Throw<InvalidWallException>();
            
        }

        [TestCase(5, 5, North, 5, 4)]
        [TestCase(5, 5, South, 5, 6)]
        public void Should_Throw_If_Wall_To_Be_Added_Is_Already_Present(
            int f_x, int f_y, Direction placement, int t_x, int t_y)
        {
            //Arrange
            var gameEnv = CreateGameEnvironment(f_x, f_y, placement, t_x, t_y).Item2;
            var from = new Vector2(f_x, f_y);

            //Act
            gameEnv.AddWall(from, placement);
            Action a = () => gameEnv.AddWall(from, placement);

            //Assert
            a.Should().Throw<WallAlreadyPresentException>();
        }

        [TestCase(5, 5, North, 5, 4)]
        [TestCase(5, 5, South, 5, 6)]
        [TestCase(3, 4, East, 4, 4)]
        public void Should_Throw_If_Wall_Added_WRT_Another_Cell_Is_Already_Present(
            int f_x, int f_y, Direction placement, int t_x, int t_y)
        {
            //Arrange
            var gameEnv = CreateGameEnvironment(f_x, f_y, placement, t_x, t_y).Item2;
            var from = new Vector2(f_x, f_y);
            var to = new Vector2(t_x, t_y);

            //Act
            gameEnv.AddWall(from, placement);
            Action a = () => gameEnv.AddWall(to, placement.Opposite());

            //Assert
            a.Should().Throw<WallAlreadyPresentException>();
        }

        [TestCase(5, 5, North, 5, 4)]
        [TestCase(5, 5, South, 5, 6)]
        [TestCase(3, 4, East, 4, 4)]
        public void Should_Correctly_Set_Walls_If_Added_Correctly(
            int f_x, int f_y, Direction dir, int t_x, int t_y)
        {
            //Arrange
            var token = CreateGameEnvironment(f_x, f_y, dir, t_x, t_y);
            var gameEnv = token.Item2;
            var board = token.Item1;
            var from = new Vector2(f_x, f_y);
            var to = new Vector2(t_x, t_y);

            //Act
            gameEnv.AddWall(from, dir);

            //Assert
            board.GetCell(from).IsAccessible(dir).Should().Be(false);
            board.GetCell(to).IsAccessible(dir.Opposite()).Should().Be(false);
        }

        [Test]
        public void Should_Correctly_Set_Multiple_Walls_Arround_A_Cell()
        {
            //Arrange
            var from = new Vector2(5, 5);
            var board = new Board();
            board.SetDimension(9);
            var wallFactory = Substitute.For<IWallFactory>();
            wallFactory.CreateWall(North, from).Returns(new Wall(North, from));
            wallFactory.CreateWall(South, from).Returns(new Wall(South, from));
            wallFactory.CreateWall(North, new Vector2(5, 6)).Returns(new Wall(North, new Vector2(5, 6)));
            wallFactory.CreateWall(South, new Vector2(5, 4)).Returns(new Wall(South, new Vector2(5, 4)));
            var gameEnv = new GameEnvironment(board, wallFactory);

            //Act
            gameEnv.AddWall(from, North);
            gameEnv.AddWall(from, South);

            //Assert
            board.GetCell(from).IsAccessible(North).Should().Be(false);
            board.GetCell(from).IsAccessible(South).Should().Be(false);
            board.Neighbors(board.GetCell(from)).Count().Should().Be(2);

            board.GetCell(new Vector2(5, 6)).IsAccessible(North).Should().Be(false);
            board.Neighbors(board.GetCell(new Vector2(5, 6))).Count().Should().Be(3);

            board.GetCell(new Vector2(5, 4)).IsAccessible(South).Should().Be(false);
            board.Neighbors(board.GetCell(new Vector2(5, 4))).Count().Should().Be(3);
        }

        [TestCase(5, 5, North)]
        [TestCase(8, 5, South)]
        public void Should_Create_And_Validate_Walls(
            int f_x, int f_y, Direction dir)
        {
            //Arrange
            var gameEnvironment = CreateGameEnvironment(f_x, f_y, dir);
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
            var gameEnvironment = CreateGameEnvironment(f_x, f_y, dir);
            var from = new Vector2(f_x, f_y);

            //Act
            Action a = () => gameEnvironment.CreateAndValidateWall(from, dir);

            //Assert
            a.Should().Throw<Exception>();
        }

        private Tuple<IBoard, IGameEnvironment> CreateGameEnvironment(int f_x, int f_y, Direction dir, int t_x, int t_y)
        {
            var board = new Board();
            board.SetDimension(9);

            var from = new Vector2(f_x, f_y);
            var to = new Vector2(t_x, t_y);

            var wallFactory = Substitute.For<IWallFactory>();
            wallFactory.CreateWall(dir, from).Returns(new Wall(dir, from));
            wallFactory.CreateWall(dir.Opposite(), to).Returns(new Wall(dir.Opposite(), to));

            return Tuple.Create<IBoard, IGameEnvironment>(board, new GameEnvironment(board, wallFactory));
        }

        private GameEnvironment CreateGameEnvironment(int f_x, int f_y, Direction dir)
        {
            var board = new Board();
            board.SetDimension(9);

            var from = new Vector2(f_x, f_y);

            var wallFactory = Substitute.For<IWallFactory>();
            wallFactory.CreateWall(dir, from).Returns(new Wall(dir, from));

            return new GameEnvironment(board, wallFactory);
        }
    }
}
