using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using se.Urbaino.ShipBattles.Domain.Repositories;
using se.Urbaino.ShipBattles.Domain.Games;
using se.Urbaino.ShipBattles.Domain.Exceptions;
using System.Collections.Generic;
using se.Urbaino.ShipBattles.Web.Hubs.Lobby;
using se.Urbaino.ShipBattles.Domain.GameItems;
using se.Urbaino.ShipBattles.Domain.Services;

namespace se.Urbaino.ShipBattles.Web.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGameManagerService GameManager;

        private PlayerDTO GetCurrentPlayer => new PlayerDTO { Id = Context.UserIdentifier, Name = Context.UserIdentifier };

        public GameHub(IGameManagerService gameManager)
        {
            GameManager = gameManager;
        }

        public async Task GetGameInfo(string gameId)
        {
            await Groups.AddToGroupAsync(gameId, Context.UserIdentifier);
            var game = GameManager.GetGame(gameId, GetCurrentPlayer.Id);

            // Testing
            game = GameManager.PlaceShip(game.Id, GetCurrentPlayer.Id, new Ship(new Coordinate(1, 0), 4, Direction.East));

            await Clients.Caller.SendAsync("Initialize", game, GetCurrentPlayer);
        }

        public async Task PlaceShip(string gameId, Ship ship)
        {
            // TODO: Exception handling
            var game = GameManager.PlaceShip(gameId, GetCurrentPlayer.Id, ship);

            await Clients.Group(gameId).SendAsync("GameUpdate", game);
        }

        public async Task Fire(string gameId, Shot shot)
        {
            // TODO: Exception handling
            var game = GameManager.Fire(gameId, GetCurrentPlayer.Id, shot);

            await Clients.Group(gameId).SendAsync("GameUpdate", game);
        }

    }
}