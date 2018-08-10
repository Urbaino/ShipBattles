using System;

namespace se.Urbaino.ShipBattles.Domain.GameItems
{
    public class Coordinate
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Coordinate(int x, int y)
        {
            if (x < 0) throw new Exception($"Invalid x-coordinate: {x}. Must be greater than or equal to zero.");
            if (y < 0) throw new Exception($"Invalid y-coordinate: {y}. Must be greater than or equal to zero.");

            X = x;
            Y = y;
        }

        public Coordinate GetNeighboringCoordinates(Direction direction)
        {
            switch (direction)
            {
                case Direction.North: return new Coordinate(X, Y - 1);
                case Direction.East: return new Coordinate(X + 1, Y);
                case Direction.West: return new Coordinate(X - 1, Y);
                case Direction.South: return new Coordinate(X, Y + 1);
                default:
                    throw new Exception($"Unhandled direction: {direction}");
            }
        }

        public override int GetHashCode()
        {
            return $"{X}+{Y}".GetHashCode();
        }

        public override bool Equals(object coord)
        {
            if (coord == null || GetType() != coord.GetType())
            {
                return false;
            }

            var other = (Coordinate)coord;
            return X == other.X && Y == other.Y;
        }

    }
}
