using NUnit.Framework;
using FluentAssertions;

using Quoridor.Core.Utils;
using static Quoridor.Core.Utils.Direction;

namespace Quoridor.Tests.Utils
{
    [TestFixture]
    public class Vector2Tests
    {

        [TestCase(5, 5, North, 5, 4)]
        [TestCase(3, 6, South, 3, 7)]
        [TestCase(6, 6, East, 7, 6)]
        [TestCase(1, 6, West, 0, 6)]
        public void Should_Correctly_Return_Pos_For_A_Dir(
            int f_x, int f_y, Direction dir, int t_x, int t_y)
        {
            //Arrange
            var from = new Vector2(f_x, f_y);
            var expected = new Vector2(t_x, t_y);

            //Act
            var newPos = from.GetPosFor(dir);

            //Assert
            newPos.Should().Be(expected);
        }

        [TestCase(5, 5, 5, 4, North)]
        [TestCase(2, 2, 2, 3, South)]
        [TestCase(1, 1, 0, 1, West)]
        [TestCase(0, 0, 1, 0, East)]
        public void Should_Correctly_Provide_Dir_For_Pos(
            int f_x, int f_y, int t_x, int t_y, Direction dir)
        {
            //Arrange
            var from = new Vector2(f_x, f_y);
            var dest = new Vector2(t_x, t_y);

            //Act
            var newDir = from.GetDirFor(dest);

            //Assert
            newDir.Should().Be(dir);
        }
    }
}
