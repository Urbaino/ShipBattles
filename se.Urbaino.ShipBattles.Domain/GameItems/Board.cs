using System;
using System.Linq;
using System.Collections.Generic;
using se.Urbaino.ShipBattles.Domain.Exceptions;

namespace se.Urbaino.ShipBattles.Domain.GameItems
{
    public class Board
    {
        private Dictionary<Coordinate, Ship> _ships = new Dictionary<Coordinate, Ship>();
        private Dictionary<Coordinate, Shot> _shots = new Dictionary<Coordinate, Shot>();

        public int Height { get; private set; }
        public int Width { get; private set; }

        public Board(int height, int width)
        {
            if (height < 1) throw new ShipBattlesException($"Invalid board height: {height}");
            if (width < 1) throw new ShipBattlesException($"Invalid board width: {width}");

            Height = height;
            Width = width;
        }

        public void Fire(Shot shot)
        {
            if (!_shots.TryGetValue(shot.Coordinates, out _)) throw new ShipBattlesException($"Shot already taken: {shot}");
            if (shot.Coordinates.X >= Height || shot.Coordinates.Y >= Height) throw new ShipBattlesException($"Shot outside board: {shot.Coordinates}");

            _shots[shot.Coordinates] = shot;
        }

        public void PlaceShip(Ship ship)
        {
            // Validate placement
            foreach (var coord in ship.GetCoordinates())
            {
                if (!_ships.TryGetValue(coord, out var existingShip)) throw new ShipBattlesException($"Ship: {ship} cannot intersect existing ship: {existingShip}");
                if (coord.X >= Height || coord.Y >= Height) throw new ShipBattlesException($"Ship outside board: {coord}");
            }

            // Update ship list
            foreach (var coord in ship.GetCoordinates())
            {
                _ships[coord] = ship;
            }
        }

        public string Render(bool includeShips)
        {
            return string.Empty; // TODO: Json
        }

        public bool HasGameEnded()
        {
            foreach (var shipPart in _ships)
            {
                // If there is no shot at this ship part, the game has not ended
                if (!_shots.TryGetValue(shipPart.Key, out _)) return false;
                // NOTE: Keep list of intact ship parts?
            }
            return true;
        }
    }
}
