using System;
using se.Urbaino.ShipBattles.Domain.GameItems;
using se.Urbaino.ShipBattles.Domain.Games;

namespace se.Urbaino.ShipBattles.Web.Hubs
{
    public class GameDTO
    {
        public GameDTO(Domain.Games.Game game, PlayerDTO player)
        {
            GameId = game.Id;
            Me = player;

            bool isPlayerA = game.PlayerA == player.Id;
            Opponent = isPlayerA ? game.PlayerB : game.PlayerA;
            Board = isPlayerA ? game.BoardA : game.BoardB;
            EnemyBoard = game.IsEnded ?
                (isPlayerA ? game.BoardB : game.BoardA) :
                (isPlayerA ? game.BoardB.OnlySunkShips() : game.BoardA.OnlySunkShips());
            GameState = isPlayerA ? game.PlayerAState : game.PlayerBState;
        }

        public string GameId { get; set; }
        public PlayerDTO Me { get; set; }
        public string Opponent { get; set; }
        public Board Board { get; set; }
        public Board EnemyBoard { get; set; }
        public GameState GameState { get; set; }

    }
}