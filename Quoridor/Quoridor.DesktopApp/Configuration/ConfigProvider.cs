using System.IO;

using Quoridor.Common.Helpers;

namespace Quoridor.DesktopApp.Configuration
{
    public class ConfigProvider : IConfigProvider
    {
        private readonly ILocalSettings _localSettings;

        public ConfigProvider(ILocalSettings localSettings)
        {
            _localSettings = localSettings;
        }

        private DesktopAppSettings _appSettings;

        public DesktopAppSettings AppSettings
        {
            get
            {
                if (_appSettings == null)
                {
                    var path = GetFullConfigPath();
                    _appSettings = XmlHelper.Deserialize<DesktopAppSettings>(path);
                }
                return _appSettings;
            }
        }

        private string GetFullConfigPath()
        {
            return Path.Combine(_localSettings.ConfigTemplatesDir, _localSettings.DesktopAppSettingsFileName);
        }

    }
}
