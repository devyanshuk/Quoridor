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

            var otherWall = obj as Wall;

            var thisWallPoints = GetAllPointsOfWall(From, Placement);
            var otherWallPoints = GetAllPointsOfWall(otherWall.From, otherWall.Placement);

            return thisWallPoints.Item1.Equals(otherWallPoints.Item1) &&
                thisWallPoints.Item2.Equals(otherWallPoints.Item2) &&
                thisWallPoints.Item3.Equals(otherWallPoints.Item3);
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

        public IWall Opposite()
        {
            return new Wall(Placement.Opposite(), From.GetPosFor(Placement));
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
            //case 1 : if mid points intersect
            var otherWallPoints = GetAllPointsOfWall(from, dir);
            var otherMidPoint = otherWallPoints.Item2;

            var thisWallPoints = GetAllPointsOfWall(From, Placement);
            var thisMidPoint = thisWallPoints.Item2;

            var midPointIntersect = otherMidPoint.Equals(thisMidPoint);

            if ((IsHorizontal() && (dir.Equals(Direction.East) || (dir.Equals(Direction.West)))) ||
                (IsVertical() && ( dir.Equals(Direction.North) || dir.Equals(Direction.South))))
            {
                return midPointIntersect;
            }

            //case 2: if both walls are vertical or horizontal and mid point of one wall intersects
            // with one of the end-points of another wall

            var sideIntersectWithMid = otherMidPoint.Equals(thisWallPoints.Item1) ||
                otherMidPoint.Equals(thisWallPoints.Item3) ||
                thisMidPoint.Equals(otherWallPoints.Item1) ||
                thisMidPoint.Equals(otherWallPoints.Item3);

            return midPointIntersect || sideIntersectWithMid;
        }

        private Tuple<Vector2, Vector2, Vector2> GetAllPointsOfWall(Vector2 from, Direction dir)
        {
            switch(dir)
            {
                case Direction.North:
                        return Tuple.Create(from, new Vector2(from.X + 1, from.Y), new Vector2(from.X + 2, from.Y));
                case Direction.South:
                        return Tuple.Create(new Vector2(from.X, from.Y + 1), new Vector2(from.X + 1, from.Y + 1), new Vector2(from.X + 2, from.Y + 1));
                case Direction.East:
                    return Tuple.Create(new Vector2(from.X + 1, from.Y), new Vector2(from.X + 1, from.Y + 1), new Vector2(from.X + 1, from.Y + 2));
                default:
                    return Tuple.Create(from, new Vector2(from.X, from.Y + 1), new Vector2(from.X, from.Y + 2));
            }
        }

        public override string ToString()
        {
            return $"{Placement}ern wall at {From}";
        }
    }
}
