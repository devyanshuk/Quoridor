using System.IO;
using Quoridor.Common.Helpers;

namespace Quoridor.ConsoleApp.Configuration
{
    public class ConfigProvider : IConfigProvider
    {
        private readonly ILocalSettings _localSettings;


        public ConfigProvider(ILocalSettings localSettings)
        {
            _localSettings = localSettings;
        }

        private BoardChars _boardChars;

        public BoardChars BoardChars
        {
            get
            {
                if (_boardChars == null)
                {
                    var path = GetFullConfigPath();
                    _boardChars = XmlHelper.Deserialize<BoardChars>(path);
                }
                return _boardChars;
            }
        }

        private string GetFullConfigPath()
        {
            return Path.Combine(_localSettings.ConfigTemplatesDir, _localSettings.BoardCharXml);
        }
    }
}
