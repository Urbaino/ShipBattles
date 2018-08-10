using System.Collections.Generic;
using se.Urbaino.ShipBattles.Domain.GameItems;
using se.Urbaino.ShipBattles.Domain.Games;

namespace se.Urbaino.ShipBattles.Domain.Services
{
    public interface IGameManagerService
    {
        Game NewGame(string playerA, string playerB);

        Game GetGame(string gameId, string playerId);
        IEnumerable<Game> GetGames(string playerId);

        Game Fire(string gameId, string playerId, Shot shot);
        Game PlaceShip(string gameId, string playerId, Ship ship);
    }
}