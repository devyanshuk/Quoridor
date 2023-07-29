using Quoridor.Core.Utils;

namespace Quoridor.Core.Extensions
{
    public static class DirectionExtensions
    {
        public static Direction Opposite(this Direction dir)
        {
            var intDir = (int)dir;
            var oppositePlacement = (Direction)(intDir % 2 == 1 ? intDir - 1 : intDir + 1);
            return oppositePlacement;
        }
    }
}
