using System;
using se.Urbaino.ShipBattles.Domain.GameItems;

namespace se.Urbaino.ShipBattles.Domain.Games
{
    public interface IGame
    {
        string Id { get; }
        GameState State { get; }

        void PlayerAPlaceShip(Ship ship);
        void PlayerBPlaceShip(Ship ship);

        void PlayerATakeShot(Shot shot);
        void PlayerBTakeShot(Shot shot);
    }
}