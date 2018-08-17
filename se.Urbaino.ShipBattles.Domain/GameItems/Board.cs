using System;
using System.Linq;
using System.Collections.Generic;
using se.Urbaino.ShipBattles.Domain.Exceptions;
using System.Collections.ObjectModel;

namespace se.Urbaino.ShipBattles.Domain.GameItems
{
    public class Board
    {
        // Data properties
        public List<Ship> Ships { get; private set; }
        public List<Shot> Shots { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        // Public properties
        // public IEnumerable<Ship> Ships => ShipList;
        // public IEnumerable<Shot> Shots => ShotList;

        public Board(int height, int width) : this(height, width, new List<Ship>(), new List<Shot>())
        {
        }

        public Board(int height, int width, List<Ship> ships, List<Shot> shots)
        {
            if (height < 1) throw new ShipBattlesException($"Invalid board height: {height}");
            if (width < 1) throw new ShipBattlesException($"Invalid board width: {width}");

            Height = height;
            Width = width;

            Ships = ships;
            Shots = shots;
        }

        public Board WithoutShips()
        {
            return new Board(Height, Width, new List<Ship>(), Shots);
        }

        public void Fire(Coordinate coordinates)
        {
            if (ShotMap().TryGetValue(coordinates, out _)) throw new ShipBattlesException($"Shot already taken: {coordinates}");
            if (coordinates.X >= Height || coordinates.Y >= Height) throw new ShipBattlesException($"Shot outside board: {coordinates}");

            var hit = ShipMap().TryGetValue(coordinates, out _);
            Shots.Add(new Shot(coordinates, hit));
        }

        public void PlaceShip(Ship ship)
        {
            // Validate placement
            var map = ShipMap();
            foreach (var coord in ship.GetCoordinates())
            {
                if (map.TryGetValue(coord, out var existingShip)) throw new ShipBattlesException($"Ship: {ship} cannot intersect existing ship: {existingShip}");
                if (coord.X >= Width || coord.Y >= Height) throw new ShipBattlesException($"Ship outside board: {coord}");
            }

            // Update ship list
            Ships.Add(ship);
        }

        public bool AreAllShipsSunk()
        {
            var map = ShotMap();
            foreach (var shipPart in ShipMap())
            {
                // If there is no shot at this ship part, the game has not ended
                if (!map.TryGetValue(shipPart.Key, out _)) return false;
                // NOTE: Keep list of intact ship parts?
            }
            return true;
        }


        // Helpers
        private Dictionary<Coordinate, Ship> ShipMap()
        {
            var coordList = Ships.SelectMany(ship => ship.GetCoordinates().Select(c => new { Coordinate = c, Ship = ship }));
            return coordList.ToDictionary(x => x.Coordinate, x => x.Ship);
        }

        private Dictionary<Coordinate, Shot> ShotMap() => Shots.ToDictionary(shot => shot.Coordinates, shot => shot);
    }
}
