using se.Urbaino.ShipBattles.Domain.GameItems;

namespace se.Urbaino.ShipBattles.Data.DBO
{
    public class ShipDBO
    {
        public int OriginX { get; set; }
        public int OriginY { get; set; }
        public int Length { get; set; }
        public Direction Heading { get; set; }

        public Ship ToDomain()
        {
            return new Ship(new Coordinate(OriginX, OriginY), Length, Heading);
        }

    }
}
