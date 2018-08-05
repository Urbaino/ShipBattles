using System.Collections.Generic;
using System.Threading.Tasks;
using se.Urbaino.ShipBattles.Domain.Games;

namespace se.Urbaino.ShipBattles.Domain.Repositories
{
    public interface IGameRepository
    {
        Task UpdateAsync(Game game);
        IEnumerable<Game> GetListOfGames(string id);
        Game GetGame(string gameId, string playerId);
    }
}