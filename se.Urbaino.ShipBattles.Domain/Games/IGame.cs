using se.Urbaino.ShipBattles.Domain.GameItems;

namespace se.Urbaino.ShipBattles.Domain.Games
{
    public interface IGame
    {
        GameState State { get; }

        IGame NewGame(string playerA, string playerB);

        void PlayerAPlaceShip(Ship ship);
        void PlayerBPlaceShip(Ship ship);

        void PlayerATakeShot(Shot shot);
        void PlayerBTakeShot(Shot shot);
    }
}