using System;
using System.IO;
using System.Xml.Serialization;

using Quoridor.Common.Logging;

namespace Quoridor.Common.Helpers
{
    public class XmlHelper
    {
        private static readonly ILogger _log = Logger.InstanceFor<XmlHelper>();

        public static T Deserialize<T>(string path)
        {
            var sr = new StreamReader(path);
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                var deserialized = (T)serializer.Deserialize(sr);
                _log.Info($"Successfully deserialized config at path '{path}' to '{typeof(T).Name}'");
                return deserialized;
            }
            catch(Exception ex)
            {
                _log.Error($"Error occurred while deserializing xml at path '{path}' : {ex.Message}");
                sr.Close();
                throw;
            }
        }
    }
}
