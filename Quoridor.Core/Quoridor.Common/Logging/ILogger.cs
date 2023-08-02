using System;

namespace Quoridor.Common.Logging
{
    public interface ILogger
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Error(string message, Exception e);
    }
}
