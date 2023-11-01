using System;
using Quoridor.Core.Extensions;
using Quoridor.Core.Utils;

namespace Quoridor.Core.Environment
{
    public class Wall : IWall
    {
        public Direction Placement { get; private set; }
        public Vector2 From { get; private set; }

        public Wall(Direction placement, Vector2 from)
        {
            Placement = placement;
            From = from;
        }

        public Wall() { }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Wall)) return false;
            var wall = obj as Wall;
            var isExactWall = wall.Placement.Equals(Placement) && wall.From.Equals(From);

            // Northern wall from (5,5) is equivalent to Southern wall from (5,4)
            // So we need to account for that too

            var oppositePOVFrom = wall.From.GetPosFor(wall.Placement);
            var oppositeDir = wall.Placement.Opposite();
            var oppositePovEquals = oppositePOVFrom.Equals(From) && oppositeDir.Equals(Placement);

            return isExactWall || oppositePovEquals;
        }

        public override int GetHashCode()
        {
            var oppositePOVFrom = From.GetPosFor(Placement);
            var oppositeDir = Placement.Opposite();

            var hashCode = Placement.GetHashCode();
            hashCode += From.GetHashCode();
            hashCode += oppositePOVFrom.GetHashCode();
            hashCode += oppositeDir.GetHashCode();

            return hashCode;
        }

        public bool IsHorizontal()
        {
            return Placement.Equals(Direction.North) || Placement.Equals(Direction.South);
        }

        public bool IsVertical()
        {
            return Placement.Equals(Direction.East) || Placement.Equals(Direction.West);
        }

        public bool Intersects(Wall other)
        {
            return Intersects(other.From, other.Placement);
        }

        // To check if two walls intersect, we first need to get the mid-point of the wall
        // Each cell coordinate represents the top-left coordinate of the board, so we need to
        // account for that
        // eg: (5,5) North wall intersects (6,4) west wall
        // eg: (5,5) south wall intersects (5,5) east wall
        public bool Intersects(Vector2 from, Direction dir)
        {
            return GetMidPoint(from, dir).Equals(GetMidPoint(From, Placement));
        }

        // (5,5) North [(6,5)] x+1      INTERSECTS (6,4) West [(6,5)] y+1
        // (5,5) South [(6,6)] x+1, y+1 INTERSECTS (6,5) West [(6,6)] y+1
        // (5,5) South [(6,6)] x+1, y+1 INTERSECTS (5,5) East [(6,6))] x+1, y+1
        private Vector2 GetMidPoint(Vector2 from, Direction dir)
        {
            var copy = from.Copy();
            switch(dir)
            {
                case Direction.North: { copy.X++; break; }
                case Direction.East:
                case Direction.South: { copy.X++; copy.Y++; break; }
                default: { copy.Y++; break; }
            }
            return copy;
        }

        public override string ToString()
        {
            return $"{Placement}ern wall at {From}";
        }
    }
}
