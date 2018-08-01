using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace se.Urbaino.ShipBattles.Web.Hubs
{
    public class LobbyHub : Hub
    {
        public async Task SendMessage(string nick)
        {
            await Clients.All.SendAsync("RecieveMessage", nick);
        }
    }
}