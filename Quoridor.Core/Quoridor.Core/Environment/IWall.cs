using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public interface IWall
    {
        Direction Placement { get; }
        Vector2 From { get; }

        bool IsHorizontal();
        bool IsVertical();

        // check if one wall intersects with the other.
        // this only happens if the centre points of two walls
        // have the same coordinate.
        // |
        // ==   => Does not intersect since the mid point of the horizontal
        // |       wall does not intersect with the mid point of the vertical wall
        // 
        // |
        //=.=   => Intersects since the mid point of the horizontal
        // |       wall intersects with the mid point of the vertical wall
        // 
        bool Intersects(Wall other);
        bool Intersects(Vector2 from, Direction dir);
    }
}
