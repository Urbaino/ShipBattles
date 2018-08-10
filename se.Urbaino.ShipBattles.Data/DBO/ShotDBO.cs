using se.Urbaino.ShipBattles.Domain.GameItems;

namespace se.Urbaino.ShipBattles.Data.DBO
{
    public class ShotDBO
    {
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }

        public Shot ToDomain()
        {
            return new Shot(new Coordinate(CoordinateX, CoordinateY));
        }

    }
}
