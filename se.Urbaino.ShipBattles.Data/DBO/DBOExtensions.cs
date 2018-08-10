using System.Linq;
using Newtonsoft.Json;
using se.Urbaino.ShipBattles.Domain.GameItems;
using se.Urbaino.ShipBattles.Domain.Games;

namespace se.Urbaino.ShipBattles.Data.DBO
{
    internal static class DBOExtensions
    {
        public static GameDBO ToDBO(this Game game)
        {
            return new GameDBO
            {
                Id = game.Id,
                Timestamp = game.Timestamp,
                HeightA = game.BoardA.Height,
                WidthA = game.BoardA.Width,
                JSONShipsA = JsonConvert.SerializeObject(game.BoardA.Ships),
                JSONShotsA = JsonConvert.SerializeObject(game.BoardA.Shots),

                HeightB = game.BoardB.Height,
                WidthB = game.BoardB.Width,
                JSONShipsB = JsonConvert.SerializeObject(game.BoardB.Ships),
                JSONShotsB = JsonConvert.SerializeObject(game.BoardB.Shots),

                PlayerA = game.PlayerA,
                PlayerB = game.PlayerB,
                PlayerAState = game.PlayerAState,
                PlayerBState = game.PlayerBState
            };
        }

        public static ShipDBO ToDBO(this Ship ship)
        {
            return new ShipDBO
            {
                OriginX = ship.Origin.X,
                OriginY = ship.Origin.Y,
                Length = ship.Length,
                Heading = ship.Heading
            };
        }

        public static ShotDBO ToDBO(this Shot shot)
        {
            return new ShotDBO
            {
                CoordinateX = shot.Coordinates.X,
                CoordinateY = shot.Coordinates.Y
            };
        }

    }
}
