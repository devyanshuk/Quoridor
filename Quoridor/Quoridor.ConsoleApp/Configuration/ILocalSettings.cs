using System;
namespace Quoridor.ConsoleApp.Configuration
{
    public interface ILocalSettings
    {
        public string ConfigTemplatesDir { get; }
        public string BoardCharXml { get; }
    }
}
