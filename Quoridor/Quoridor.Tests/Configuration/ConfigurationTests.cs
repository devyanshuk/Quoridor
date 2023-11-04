using System.IO;
using NUnit.Framework;
using FluentAssertions;

using Quoridor.Common.Helpers;
using Quoridor.ConsoleApp.Configuration;

namespace Quoridor.Tests.Configuration
{
    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        public void Should_Deserialize_BoardChars_Correctly()
        {
            //Arrange
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "ConfigTemplates", "BoardCharacters.xml");

            //Act
            var boardChars = XmlHelper.Deserialize<BoardChars>(path);

            //Assert
            boardChars.BorderSeparator.HorizontalBorderSeparator.Should().NotBeEmpty();
            boardChars.BorderSeparator.VerticalBorderSeparator.Should().NotBeEmpty();
            boardChars.BorderSeparator.IntersectionBorderSeparator.Should().NotBeEmpty();

            boardChars.WallSeparator.HorizontalWallSeparator.Should().NotBeEmpty();
            boardChars.WallSeparator.VerticalWallSeparator.Should().NotBeEmpty();
        }
    }
}
