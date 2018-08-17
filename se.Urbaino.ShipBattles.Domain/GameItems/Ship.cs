using System;
using System.Collections.Generic;

namespace se.Urbaino.ShipBattles.Domain.GameItems
{
    public class Ship
    {
        public Coordinate Origin { get; private set; }
        public int Length { get; private set; }
        public Direction Heading { get; private set; }

        public Ship(Coordinate origin, int length, Direction heading)
        {
            if (length < 1) throw new Exception($"Invalid length: {length}. Must be greater than zero.");

            Origin = origin;
            Length = length;
            Heading = heading;
        }

        public IEnumerable<Coordinate> GetCoordinates()
        {
            yield return Origin;
            var coords = Origin;
            for (var i = 1; i < Length; ++i)
            {
                coords = coords.GetNeighboringCoordinates(Heading);
                yield return coords;
            }
        }

    }
}
