using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;

using Quoridor.Core.Utils;
using Quoridor.Core.Environment;
using Quoridor.Core.Utils.CustomExceptions;
using static Quoridor.Core.Utils.Direction;
using System.Collections.Generic;

namespace Quoridor.Tests.Environment
{
    [TestFixture]
    public class BoardTests
    {
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void Should_Throw_If_Incorrect_Dimension_Provided(int dimension)
        {
            //Arrange
            var board = new Board();

            //Act
            Action act = () => board.SetDimension(dimension);

            //Assert
            act.Should().Throw<Exception>();
        }

        [TestCase(9)]
        [TestCase(11)]
        public void Should_Correctly_Initialize_Cells_On_Correct_Dimension(int dimension)
        {
            //Arrange
            var board = new Board();

            //Act
            board.SetDimension(dimension);

            //Assert
            for (int i = 0; i < dimension; i++)
                for (int j = 0; j < dimension; j++)
                {
                    board.Cells[i, j].Position.X.Should().Be(i);
                    board.Cells[i, j].Position.Y.Should().Be(j);
                }
        }

        [TestCase(5, 5, 3, North)]
        [TestCase(0, 0, 1, South)]
        [TestCase(5, 5, 2, North, South)]
        [TestCase(0, 0, 0, South, East)]
        public void Should_Return_Correct_Movable_Space_From_A_Cell(
            int x, int y, int expectedCount, params Direction[] directions)
        {
            //Arrange
            var board = new Board();
            board.SetDimension(9);
            var refCell = board.Cells[x, y];
            foreach (var dir in directions)
                refCell.Walls[(int)dir] = new Wall();

            //Act
            var neighbors = board.Neighbors(refCell);

            //Assert
            neighbors.Count().Should().Be(expectedCount);
        }

        [TestCase(5, 5, 8, 8)]
        [TestCase(0, 0, 0, 3)]
        [TestCase(5, 5, 5, 8)]
        public void Should_Throw_If_Incorrect_Wall_Is_Placed(
            int f_x, int f_y, int t_x, int t_y)
        {
            //Arrange
            var board = new Board();
            board.SetDimension(9);

            //Act
            Action a = () => board.AddWall(new Vector2(f_x, f_y), new Vector2(t_x, t_y));

            //Assert
            a.Should().Throw<InvalidWallException>();
        }

        [TestCase(5, 5, 5, 7, Placement.Horizontal)]
        [TestCase(0, 0, 0, 2, Placement.Horizontal)]
        [TestCase(5, 5, 7, 5, Placement.Vertical)]
        public void Should_Throw_If_Wall_To_Be_Added_Is_Already_Present(
            int f_x, int f_y, int t_x, int t_y, Placement placement)
        {
            //Arrange
            var board = new Board();
            board.SetDimension(9);
            var from = new Vector2(f_x, f_y);
            var to = new Vector2(t_x, t_y);
            var wall = new Wall(placement, from, to);
            board.Walls.Add(wall);

            //Act
            Action a = () => board.AddWall(from, to);

            //Assert
            a.Should().Throw<WallAlreadyPresentException>();
        }
    }
}
