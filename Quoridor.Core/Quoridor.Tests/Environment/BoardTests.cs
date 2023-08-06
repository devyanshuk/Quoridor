using System;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

using Quoridor.Core.Utils;
using Quoridor.Core.Environment;
using static Quoridor.Core.Utils.Direction;

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
        [TestCase(0, 0, 2)]
        [TestCase(5, 5, 4)]
        [TestCase(8, 8, 2)]
        [TestCase(4, 8, 3)]
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

        [TestCase(-1, 0, false)]
        [TestCase(5, 5, true)]
        [TestCase(8, 8, true)]
        [TestCase(9, 7, false)]
        public void Should_Correctly_Decide_If_Pos_Is_Within_Board_Bounds(
            int f_x, int f_y, bool expected)
        {
            //Arrange
            var board = new Board();
            board.SetDimension(9);
            var pos = new Vector2(f_x, f_y);

            //Act
            //Assert
            board.WithinBounds(pos).Should().Be(expected);
        }
    }
}
