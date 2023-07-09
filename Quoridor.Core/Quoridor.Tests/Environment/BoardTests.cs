using System;
using NUnit.Framework;
using FluentAssertions;
using NSubstitute;

using Quoridor.Core.Environment;
using Quoridor.Core.Utils;
using static Quoridor.Core.Utils.Direction;
using System.Linq;

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
            var board = new Board(null);

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
            var board = new Board(null);

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
        public void Should_Return_Correct_Movable_Space_From_A_Cell(int x, int y, int expectedCount, params Direction[] directions)
        {
            //Arrange
            var wallFactory = Substitute.For<IWallFactory>();
            var board = new Board(wallFactory);
            board.SetDimension(9);
            var refCell = board.Cells[x, y];
            foreach (var dir in directions)
                refCell.Walls[(int)dir] = new Wall();

            //Act
            var neighbors = board.Neighbors(refCell);

            //Assert
            neighbors.Count().Should().Be(expectedCount);
        }
    }
}
