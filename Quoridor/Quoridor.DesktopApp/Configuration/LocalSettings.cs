using System.Configuration;
using System.Collections.Specialized;

namespace Quoridor.DesktopApp.Configuration
{
    public class LocalSettings : ILocalSettings
    {
        private readonly NameValueCollection _appSettings = ConfigurationManager.AppSettings;

        public string ConfigTemplatesDir => _appSettings[nameof(ConfigTemplatesDir)];

        public string DesktopAppSettingsFileName => _appSettings[nameof(DesktopAppSettingsFileName)];
    }
}
