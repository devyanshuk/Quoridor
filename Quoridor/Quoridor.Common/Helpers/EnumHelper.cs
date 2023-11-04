using System;
using Quoridor.Common.Logging;

namespace Quoridor.Common.Helpers
{
    public class EnumHelper
    {
        private static readonly ILogger _log = Logger.InstanceFor<EnumHelper>();

        public static T ParseEnum<T>(string value)
        {
            try
            {
                return (T)Enum.Parse(enumType: typeof(T), value: value, ignoreCase: true);
            }
            catch(Exception ex)
            {
                var message = @$"Cannot parse {value} to {typeof(T).Name}. Available values are {
                    String.Join(",", Enum.GetNames(typeof(T)))}";
                _log.Error(message);
                throw new ArgumentException(message, ex);
            }
        }
    }
}
