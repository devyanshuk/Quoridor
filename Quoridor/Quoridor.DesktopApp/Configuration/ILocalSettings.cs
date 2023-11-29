namespace Quoridor.DesktopApp.Configuration
{
    public interface ILocalSettings
    {
        string ConfigTemplatesDir { get; }
        string DesktopAppSettingsFileName { get; }
    }
}
