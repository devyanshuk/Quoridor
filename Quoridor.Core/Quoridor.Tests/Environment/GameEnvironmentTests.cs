using System;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;

using Quoridor.Core.Utils;
using Quoridor.Core.Extensions;
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
            var gameEnv = CreateGameEnvironment(f_x, f_y, placement, t_x, t_y);

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
            var gameEnv = CreateGameEnvironment(f_x, f_y, placement, t_x, t_y);
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
            var gameEnv = CreateGameEnvironment(f_x, f_y, placement, t_x, t_y);
            var from = new Vector2(f_x, f_y);
            var to = new Vector2(t_x, t_y);

            //Act
            gameEnv.AddWall(from, placement);
            Action a = () => gameEnv.AddWall(to, placement.Opposite());

            //Assert
            a.Should().Throw<WallAlreadyPresentException>();
        }

        private IGameEnvironment CreateGameEnvironment(int f_x, int f_y, Direction dir, int t_x, int t_y)
        {
            var board = new Board();
            board.SetDimension(9);

            var from = new Vector2(f_x, f_y);
            var to = new Vector2(t_x, t_y);

            var wallFactory = Substitute.For<IWallFactory>();
            wallFactory.CreateWall(dir, from).Returns(new Wall(dir, from));
            wallFactory.CreateWall(dir.Opposite(), to).Returns(new Wall(dir.Opposite(), to));

            return new GameEnvironment(board, wallFactory);
        }
    }
}
