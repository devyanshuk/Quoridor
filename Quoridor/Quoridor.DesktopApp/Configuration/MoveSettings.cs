using System;
using System.Xml.Serialization;

using Quoridor.Core;
using Quoridor.Core.Utils;
using Quoridor.Common.Helpers;
using Quoridor.Core.Environment;

namespace Quoridor.DesktopApp.Configuration
{
    [XmlInclude(typeof(WallMove))]
    [XmlInclude(typeof(PlayerMove))]
    public abstract class MoveInfo
    {
        public abstract Movement GetMovement();
    }

    [Serializable]
    public class WallMove : MoveInfo
    {
        [XmlAttribute(nameof(X))]
        public int X { get; set; }

        [XmlAttribute(nameof(Y))]
        public int Y { get; set; }

        [XmlAttribute(nameof(Placement))]
        public string _placement { get; set; }

        [XmlIgnore]
        public Direction Placement => EnumHelper.ParseEnum<Direction>(_placement);

        public override Movement GetMovement()
        {
            return new Wall(Placement, new(X, Y));
        }
    }

    [Serializable]
    public class PlayerMove : MoveInfo
    {
        [XmlAttribute(nameof(X))]
        public int X { get; set; }

        [XmlAttribute(nameof(Y))]
        public int Y { get; set; }

        public override Movement GetMovement()
        {
            return new Vector2(X, Y);
        }
    }

}
