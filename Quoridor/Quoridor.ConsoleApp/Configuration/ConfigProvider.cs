using Quoridor.Common.Helpers;

namespace Quoridor.ConsoleApp.Configuration
{
    public class ConfigProvider : IConfigProvider
    {
        private BoardChars _boardChars;

        public ConfigProvider()
        {
        }

        public BoardChars BoardChars
        {
            get
            {
                if (_boardChars == null)
                {
                    var path = GetFullConfigPathFor("BoardCharacters.xml");
                    _boardChars = XmlHelper.Deserialize<BoardChars>(path);
                }
                return _boardChars;
            }
        }

        private string GetFullConfigPathFor(string file)
        {
#if _WINDOWS
            return $"ConfigTemplates\\{file}";
#else
            return $"ConfigTemplates/{file}";
#endif
        }


    }
}
