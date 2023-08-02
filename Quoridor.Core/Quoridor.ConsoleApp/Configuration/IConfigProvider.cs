using System;
namespace Quoridor.ConsoleApp.Configuration
{
    public interface IConfigProvider
    {
        BoardChars BoardChars { get; }
    }
}
