using System;
using System.Collections.Generic;

namespace se.Urbaino.ShipBattles.Domain.GameItems
{
    public class Ship
    {
        public Coordinate Origin { get; }
        public int Length { get; }
        public Direction Heading { get; }

        public Ship(Coordinate origin, int length, Direction heading)
        {
            if (length < 1) throw new Exception($"Invalid length: {length}. Must be greater than zero.");

            Origin = origin;
            Length = length;
            Heading = heading;
        }

        public IEnumerable<Coordinate> GetCoordinates()
        {
            var coords = Origin;
            for (var i = 0; i < Length; ++i)
            {
                yield return coords;
                coords = coords.GetNeighboringCoordinates(Heading);
            }
        }

        public override string ToString()
        {
            // TODO: Json-blob
            return string.Empty;
        }
    }
}
