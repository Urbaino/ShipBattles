using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using se.Urbaino.ShipBattles.Domain.Repositories;
using se.Urbaino.ShipBattles.Domain.Games;
using se.Urbaino.ShipBattles.Domain.Exceptions;
using System.Collections.Generic;
using se.Urbaino.ShipBattles.Web.Hubs.Lobby;
using System.Linq;
using se.Urbaino.ShipBattles.Domain.Services;

namespace se.Urbaino.ShipBattles.Web.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly IGameManagerService GameManager;
        private static List<PlayerDTO> ActiveUsers = new List<PlayerDTO>();

        private static PlayerDTO GetCurrentPlayer(HubCallerContext context) => new PlayerDTO { Id = context.UserIdentifier, Name = context.UserIdentifier };

        public LobbyHub(IGameManagerService gameManager)
        {
            GameManager = gameManager;
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("RecieveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.UserIdentifier);
            var player = GetCurrentPlayer(Context);
            await Clients.Others.SendAsync("NewPlayer", player);

            var dbGames = GameManager.GetGames(player.Id);
            var games = dbGames.Select(g => new GameDTO
            {
                Id = g.Id,
                OpponentName = (player.Id == g.PlayerA ? g.PlayerB : g.PlayerA),
                ResultIsVictory = (g.PlayerAState == GameState.Win || g.PlayerBState == GameState.Win) ?
                     (player.Id == g.PlayerA ? g.PlayerAState == GameState.Win : g.PlayerBState == GameState.Win) :
                     (bool?)null
            });
            await Clients.Caller.SendAsync("Initialize", player, ActiveUsers, games);

            ActiveUsers.Add(player);
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.UserIdentifier);
            await Clients.Others.SendAsync("PlayerDisconnected", Context.UserIdentifier);

            ActiveUsers.Remove(GetCurrentPlayer(Context));
        }

        public async Task ChallengePlayer(string playerB)
        {
            var challenge = new Challenge(Context.UserIdentifier, playerB);
            await Clients.Group(playerB).SendAsync("ChallengeReceived", challenge, challenge.Encrypt());
        }

        public async Task AcceptChallenge(string state)
        {
            var challenge = Challenge.DecryptAndVerify(state, Context.UserIdentifier);
            var game = GameManager.NewGame(challenge.PlayerA, challenge.PlayerB);

            await Clients.Caller.SendAsync("GameOn", game.Id);
            await Clients.Group(challenge.PlayerA).SendAsync("GameOn", game.Id);
        }

        public async Task DeclineChallenge(string state)
        {
            var challenge = Challenge.DecryptAndVerify(state, Context.UserIdentifier);
            await Clients.Group(challenge.PlayerA).SendAsync("NoGame", challenge.PlayerB);
        }

        private class Challenge
        {
            public readonly string PlayerA;
            public readonly string PlayerB;
            public readonly DateTime Timestamp = DateTime.Now;

            public Challenge(string playerA, string playerB)
            {
                PlayerA = playerA;
                PlayerB = playerB;
            }

            public string Encrypt()
            {
                var data = JsonConvert.SerializeObject(this);
                var cipherData = data; // TODO: Encrypt
                return cipherData;
            }

            public static Challenge DecryptAndVerify(string cipherData, string playerB)
            {
                var data = cipherData; // TODO: Decrypt
                var challenge = JsonConvert.DeserializeObject<Challenge>(data);
                if (challenge.PlayerB != playerB) throw new ShipBattlesException("Challenge not applicable");
                return challenge;
            }

        }
    }
}