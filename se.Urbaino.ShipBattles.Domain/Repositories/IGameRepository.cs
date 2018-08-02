using se.Urbaino.ShipBattles.Domain.Games;

namespace se.Urbaino.ShipBattles.Domain.Repositories
{
    public interface IGameRepository
    {
        void Update(IGame game);
    }
}