using NUnit.Framework;
using FluentAssertions;

using Quoridor.Core.Utils;
using Quoridor.Core.Extensions;
using static Quoridor.Core.Utils.Direction;

namespace Quoridor.Tests.Extensions
{
    [TestFixture]
    public class DirectionExtensionsTests
    {

        [TestCase(North, South)]
        [TestCase(South, North)]
        [TestCase(East, West)]
        [TestCase(West, East)]
        public void Should_Return_Correct_Opposite_Dir(Direction dir, Direction expected)
        {
            //Arrange
            //Act
            var oppDir = dir.Opposite();

            //Assert
            oppDir.Should().Be(expected);
        }
    }
}
