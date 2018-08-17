using System;

namespace se.Urbaino.ShipBattles.Domain.GameItems
{
    public class Shot
    {
        public Coordinate Coordinates { get; }
        public bool Hit { get; }

        public Shot(Coordinate coordinates, bool hit)
        {
            Coordinates = coordinates;
            Hit = hit;
        }

    }
}
