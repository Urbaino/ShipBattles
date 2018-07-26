using System;

namespace se.Urbaino.ShipBattles.Domain
{
    public class Ship
    {
        public Coordinate Coordinates {get;}
        public int Length {get;}
        public Direction Heading {get;}

        public Ship(Coordinate coordinates, int length, Direction heading)
        {
            if (length < 1) throw new Exception($"Invalid length: {length}. Must be greater than zero.");
            
            Coordinates = coordinates;
            Length = length;
            Heading=  heading;
        }

        public override string ToString(){
            // TODO: Json-blob
            return string.Empty;
        }
    }
}
