using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using se.Urbaino.ShipBattles.Domain.GameItems;
using se.Urbaino.ShipBattles.Domain.Games;

namespace se.Urbaino.ShipBattles.Data.DBO
{
    internal class GameDBO
    {
        // Game
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }

        public string PlayerA { get; set; }
        public string PlayerB { get; set; }

        public GameState PlayerAState { get; set; }
        public GameState PlayerBState { get; set; }

        public Game ToDomain()
        {
            return new Game(Id, Timestamp, ToDomainBoardA(), ToDomainBoardB(), PlayerA, PlayerB, PlayerAState, PlayerBState);
        }

        // Board A
        public string JSONShipsA { get; set; }
        public string JSONShotsA { get; set; }
        public int HeightA { get; set; }
        public int WidthA { get; set; }

        public Board ToDomainBoardA()
        {
            return new Board(
                HeightA,
                WidthA,
                JsonConvert.DeserializeObject<List<Ship>>(JSONShipsA),
                JsonConvert.DeserializeObject<List<Shot>>(JSONShotsA)
            );
        }

        // Board B
        public string JSONShipsB { get; set; }
        public string JSONShotsB { get; set; }
        public int HeightB { get; set; }
        public int WidthB { get; set; }

        public Board ToDomainBoardB()
        {
            return new Board(
                HeightB,
                WidthB,
                JsonConvert.DeserializeObject<List<Ship>>(JSONShipsB),
                JsonConvert.DeserializeObject<List<Shot>>(JSONShotsB)
            );
        }

        internal void CopyValuesFromDomainValue(Game game)
        {
            var gameDBO = game.ToDBO();

            Timestamp = gameDBO.Timestamp;

            if (PlayerAState != gameDBO.PlayerAState)
                PlayerAState = gameDBO.PlayerAState;
            if (PlayerBState != gameDBO.PlayerBState)
                PlayerBState = gameDBO.PlayerBState;

            if (JSONShipsA != gameDBO.JSONShipsA)
                JSONShipsA = gameDBO.JSONShipsA;
            if (JSONShotsA != gameDBO.JSONShotsA)
                JSONShotsA = gameDBO.JSONShotsA;

            if (JSONShipsB != gameDBO.JSONShipsB)
                JSONShipsB = gameDBO.JSONShipsB;
            if (JSONShotsB != gameDBO.JSONShotsB)
                JSONShotsB = gameDBO.JSONShotsB;
        }
    }
}