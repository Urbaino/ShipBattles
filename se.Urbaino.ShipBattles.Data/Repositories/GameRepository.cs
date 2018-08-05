using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using se.Urbaino.ShipBattles.Domain.Games;
using se.Urbaino.ShipBattles.Domain.Repositories;

namespace se.Urbaino.ShipBattles.Data.Repositories
{
    public class GameRepository : IGameRepository
    {

        private readonly ShipBattlesContext Context;

        public GameRepository(ShipBattlesContext context)
        {
            Context = context;
        }

        Game IGameRepository.GetGame(string gameId, string playerId)
        {
            return Context.Games.FirstOrDefault(g => g.Id == gameId && (g.PlayerA == playerId || g.PlayerB == playerId));
        }

        IEnumerable<Game> IGameRepository.GetListOfGames(string playerId)
        {
            return Context.Games.Where(x => x.PlayerA == playerId || x.PlayerB == playerId).ToArray();
        }

        Task IGameRepository.UpdateAsync(Game game)
        {
            var dbGame = Context.Games.FirstOrDefault(g => g.Id == game.Id);
            if (dbGame == null)
            {
                Context.Games.Add(game);
            }
            else
            {
                dbGame = game;
            }
            Context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}