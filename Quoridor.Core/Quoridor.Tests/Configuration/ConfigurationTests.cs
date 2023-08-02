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
            boardChars.BorderSeparator.Horizontal.Should().NotBeEmpty();
            boardChars.BorderSeparator.Vertical.Should().NotBeEmpty();
            boardChars.BorderSeparator.Intersection.Should().NotBeEmpty();

            boardChars.WallSeparator.Horizontal.Should().NotBeEmpty();
            boardChars.WallSeparator.Vertical.Should().NotBeEmpty();
        }
    }
}
