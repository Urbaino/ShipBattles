using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using se.Urbaino.ShipBattles.Data.DBO;
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
            return Context.Games.FirstOrDefault(g => g.Id == gameId && (g.PlayerA == playerId || g.PlayerB == playerId)).ToDomain();
        }

        IEnumerable<Game> IGameRepository.GetListOfGames(string playerId)
        {
            return Context.Games.Where(x => x.PlayerA == playerId || x.PlayerB == playerId).Select(x => x.ToDomain()).ToArray();
        }

        void IGameRepository.Update(Game game)
        {
            var dbGame = Context.Games.FirstOrDefault(g => g.Id == game.Id);
            if (dbGame == null)
            {
                Context.Games.Add(game.ToDBO());
            }
            else
            {
                dbGame.CopyValuesFromDomainValue(game);
            }
            var saves = Context.SaveChanges();
        }
    }
}