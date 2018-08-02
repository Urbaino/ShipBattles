using System;
using se.Urbaino.ShipBattles.Domain;
using se.Urbaino.ShipBattles.Domain.Exceptions;
using se.Urbaino.ShipBattles.Domain.GameItems;

namespace se.Urbaino.ShipBattles.Domain.Games
{
    public class Game : IGame // NOTE: Make generic for rule set and determine state from Boards (no need to change state)?
    {
        private Board BoardA;
        private Board BoardB;

        private string PlayerA;
        private string PlayerB;

        public GameState State { get; private set; }

        private Game(string playerA, string playerB)
        {
            PlayerA = playerA;
            PlayerB = playerB;

            BoardA = new Board(10, 10);
            BoardB = new Board(10, 10);
        }

        IGame IGame.NewGame(string playerA, string playerB) => new Game(playerA, playerB);

        void IGame.PlayerAPlaceShip(Ship ship)
        {
            if (!IsPlayerAsTurn()) throw new ShipBattlesOutOfTurnException();
            PlaceShip(ship);
        }

        void IGame.PlayerATakeShot(Shot shot)
        {
            if (!IsPlayerAsTurn()) throw new ShipBattlesOutOfTurnException();
            TakeShot(shot);
        }

        void IGame.PlayerBPlaceShip(Ship ship)
        {
            if (IsPlayerAsTurn()) throw new ShipBattlesOutOfTurnException();
            PlaceShip(ship);
        }

        void IGame.PlayerBTakeShot(Shot shot)
        {
            if (IsPlayerAsTurn()) throw new ShipBattlesOutOfTurnException();
            TakeShot(shot);
        }

        private void PlaceShip(Ship ship)
        {
            void PlaceShipHelper(int length, Board board)
            {
                if (ship.Length != length)
                    throw new ShipBattlesIllegalMoveException($"Must place ship of length {length}");
                board.PlaceShip(ship);
            }

            switch (State)
            {
                case GameState.PlayerAPlaceShip1:
                    PlaceShipHelper(1, BoardA);
                    State = GameState.PlayerBPlaceShip1;
                    break;
                case GameState.PlayerAPlaceShip2:
                    PlaceShipHelper(2, BoardA);
                    State = GameState.PlayerBPlaceShip2;
                    break;
                case GameState.PlayerAPlaceShip3:
                    PlaceShipHelper(3, BoardA);
                    State = GameState.PlayerBPlaceShip3;
                    break;
                case GameState.PlayerAPlaceShip4:
                    PlaceShipHelper(4, BoardA);
                    State = GameState.PlayerBPlaceShip4;
                    break;

                case GameState.PlayerBPlaceShip1:
                    PlaceShipHelper(1, BoardB);
                    State = GameState.PlayerAFire;
                    break;
                case GameState.PlayerBPlaceShip2:
                    PlaceShipHelper(2, BoardB);
                    State = GameState.PlayerAPlaceShip1;
                    break;
                case GameState.PlayerBPlaceShip3:
                    PlaceShipHelper(3, BoardB);
                    State = GameState.PlayerAPlaceShip2;
                    break;
                case GameState.PlayerBPlaceShip4:
                    PlaceShipHelper(4, BoardB);
                    State = GameState.PlayerAPlaceShip3;
                    break;

                case GameState.PlayerAFire:
                case GameState.PlayerBFire:
                    throw new ShipBattlesIllegalMoveException("Cannot fire at this point");

                default:
                    throw new System.NotImplementedException();
            }
        }

        private void TakeShot(Shot shot)
        {
            switch (State)
            {
                case GameState.PlayerAPlaceShip1:
                case GameState.PlayerAPlaceShip2:
                case GameState.PlayerAPlaceShip3:
                case GameState.PlayerAPlaceShip4:
                case GameState.PlayerBPlaceShip1:
                case GameState.PlayerBPlaceShip2:
                case GameState.PlayerBPlaceShip3:
                case GameState.PlayerBPlaceShip4:
                    throw new ShipBattlesIllegalMoveException("Cannot place ship at this point");

                case GameState.PlayerAFire:
                    BoardB.Fire(shot);
                    State = BoardB.HasGameEnded() ? GameState.PlayerAWin : GameState.PlayerBFire;
                    break;
                case GameState.PlayerBFire:
                    BoardA.Fire(shot);
                    State = BoardA.HasGameEnded() ? GameState.PlayerBWin : GameState.PlayerAFire;
                    break;

                default:
                    throw new System.NotImplementedException();
            }
        }

        private bool IsPlayerAsTurn()
        {
            switch (State)
            {
                case GameState.PlayerAFire:
                case GameState.PlayerAPlaceShip1:
                case GameState.PlayerAPlaceShip2:
                case GameState.PlayerAPlaceShip3:
                case GameState.PlayerAPlaceShip4:
                    return true;

                case GameState.PlayerBFire:
                case GameState.PlayerBPlaceShip1:
                case GameState.PlayerBPlaceShip2:
                case GameState.PlayerBPlaceShip3:
                case GameState.PlayerBPlaceShip4:
                    return false;

                case GameState.PlayerAWin:
                case GameState.PlayerBWin:
                    throw new ShipBattlesGameOverException();

                default:
                    throw new System.NotImplementedException();
            }
        }
    }
}