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
            await Groups.AddToGroupAsync(Context.ConnectionId, GetCurrentPlayer.Id);
            var game = GameManager.GetGame(gameId, GetCurrentPlayer.Id);

            await Clients.Caller.SendAsync("GameUpdate", new GameDTO(game, GetCurrentPlayer));
        }

        public async Task PlaceShip(string gameId, Ship ship)
        {
            Game game;
            try
            {
                game = GameManager.PlaceShip(gameId, GetCurrentPlayer.Id, ship);
            }
            catch (ShipBattlesException ex)
            {
                await Clients.Caller.SendAsync("Message", ex.Message);
                return;
            }

            await Clients.Caller.SendAsync("GameUpdate", new GameDTO(game, GetCurrentPlayer));
            var opponent = GetCurrentPlayer.Id == game.PlayerA ?
                new PlayerDTO { Id = game.PlayerB, Name = game.PlayerB } :
                new PlayerDTO { Id = game.PlayerA, Name = game.PlayerA };
            await Clients.Group(opponent.Id).SendAsync("GameUpdate", new GameDTO(game, opponent));
        }

        public async Task Fire(string gameId, Shot shot)
        {
            Game game;
            try
            {
                game = GameManager.Fire(gameId, GetCurrentPlayer.Id, shot);
            }
            catch (ShipBattlesException ex)
            {
                await Clients.Caller.SendAsync("Message", ex.Message);
                return;
            }

            await Clients.Caller.SendAsync("GameUpdate", new GameDTO(game, GetCurrentPlayer));
            var opponent = GetCurrentPlayer.Id == game.PlayerA ?
                new PlayerDTO { Id = game.PlayerB, Name = game.PlayerB } :
                new PlayerDTO { Id = game.PlayerA, Name = game.PlayerA };
            await Clients.Group(opponent.Id).SendAsync("GameUpdate", new GameDTO(game, opponent));
        }

    }
}