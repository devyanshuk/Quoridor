using System;
using log4net;

namespace Quoridor.Common.Logging
{
    public class Logger : ILogger
    {
        private readonly ILog _log;
        private readonly string _name;

        public Logger(string loggerName)
        {
            _log = LogManager.GetLogger(loggerName);
            _name = loggerName;
        }

        public static Logger InstanceFor<T>() where T : class
        {
            return InstanceFor(typeof(T));
        }

        public static Logger InstanceFor(Type type)
        {
            return new Logger(type.Name);
        }

        public void Error(string message)
        {
            _log.Error(Format(message));
        }

        public void Info(string message)
        {
            _log.Info(Format(message));
        }

        public void Warn(string message)
        {
            _log.Warn(Format(message));
        }

        public void Error(string message, Exception e)
        {
            _log.Error(Format(message), e);
        }

        private string Format(string message)
        {
            return $"{_name}: {message}";
        }
    }
}
