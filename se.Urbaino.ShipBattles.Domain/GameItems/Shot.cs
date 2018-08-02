using System;

namespace se.Urbaino.ShipBattles.Domain.GameItems
{
    public class Shot
    {
        public Coordinate Coordinates {get;}
        
        public Shot(Coordinate coordinates)
        {
            Coordinates = coordinates;
        }

        public override string ToString(){
            // TODO: Json-blob
            return string.Empty;
        }
    }
}
