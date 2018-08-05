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

namespace se.Urbaino.ShipBattles.Web.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGameRepository GameRepository;

        private static PlayerDTO GetCurrentPlayer(HubCallerContext context) => new PlayerDTO { Id = context.UserIdentifier, Name = context.UserIdentifier };

        // public LobbyHub(IGameRepository gameRepository)
        // {
        //     GameRepository = gameRepository;
        // }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("RecieveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
        }

        public async Task PlaceShip(string gameId, Ship ship)
        {
            var currentGame = GameRepository.GetGame(gameId, GetCurrentPlayer(Context).Id);
        }

        public async Task Fire(string gameId, Shot shot)
        {

        }

    }
}