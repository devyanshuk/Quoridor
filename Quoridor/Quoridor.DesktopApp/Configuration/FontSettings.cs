using System;
using System.Xml.Serialization;

namespace Quoridor.DesktopApp.Configuration
{
    [Serializable]
    public class FontSettings
    {
        [XmlElement(nameof(PlayerFont))]
        public string PlayerFont { get; set; }
    }
}
