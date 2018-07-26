using System;
using System.Linq;
using System.Collections.Generic;

namespace se.Urbaino.ShipBattles.Domain
{
    public class Board
    {
        private Dictionary<Coordinate, Ship> _ships = new Dictionary<Coordinate, Ship>(); 
        private Dictionary<Coordinate, Shot> _shots = new Dictionary<Coordinate, Shot>(); 
        
        public int Height {get;}
        public int Width {get;}

        public Board(int height, int width){
            if (height<1) throw new Exception($"Invalid board height: {height}");
            if (width<1) throw new Exception($"Invalid board width: {width}");

            Height = height;
            Width = width;
        }

        public void Fire(Shot shot){
            if (!_shots.TryGetValue(shot.Coordinates, out _)) throw new Exception($"Shot already taken: {shot}");
            if (shot.Coordinates.X >= Height || shot.Coordinates.Y >= Height) throw new Exception($"Shot outside board: {shot.Coordinates}");
            
            _shots[shot.Coordinates] = shot;
        }

        public void PlaceShip(Ship ship){
            var coords = ship.Coordinates;
            for(var i=0; i<ship.Length; ++i){
                if (!_ships.TryGetValue(coords, out var existingShip)) throw new Exception($"Ship: {ship} cannot intersect existing ship: {existingShip}");
                coords.GetNeighboringCoordinates(ship.Heading);
                if (coords.X >= Height || coords.Y >= Height) throw new Exception($"Ship outside board: {coords}");
            }

            coords = ship.Coordinates;
            for(var i=0; i<ship.Length; ++i){
                coords.GetNeighboringCoordinates(ship.Heading);
            _ships[ship.Coordinates] = ship; 
            }
        }

        public string Render(bool includeShips)
        {
            return string.Empty; // TODO: Json
        }
    }
}
