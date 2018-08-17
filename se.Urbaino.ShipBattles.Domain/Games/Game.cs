using System;
using se.Urbaino.ShipBattles.Domain;
using se.Urbaino.ShipBattles.Domain.Exceptions;
using se.Urbaino.ShipBattles.Domain.GameItems;

namespace se.Urbaino.ShipBattles.Domain.Games
{
    public class Game // NOTE: Make generic for rule set and determine state from Boards (no need to change state)?
    {
        public string Id { get; private set; }
        public DateTime Timestamp { get; internal set; }

        public Board BoardA { get; private set; }
        public Board BoardB { get; private set; }

        public string PlayerA { get; private set; }
        public string PlayerB { get; private set; }

        public GameState PlayerAState { get; private set; }
        public GameState PlayerBState { get; private set; }

        public bool IsEnded => PlayerAState == GameState.Win || PlayerBState == GameState.Win;

        private Game(string playerA, string playerB)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;

            PlayerA = playerA;
            PlayerB = playerB;

            BoardA = new Board(10, 10);
            BoardB = new Board(10, 10);

            PlayerAState = GameState.PlaceShip4;
            PlayerBState = GameState.PlaceShip4;
        }

        /// <summary>
        /// Do not use to create a new game, use static method <see cref="NewGame"> instead.
        /// </summary>
        public Game(string id, DateTime timestamp, Board boardA, Board boardB, string playerA, string playerB, GameState playerAState, GameState playerBState)
        {
            Id = id;
            Timestamp = timestamp;
            BoardA = boardA;
            BoardB = boardB;
            PlayerA = playerA;
            PlayerB = playerB;
            PlayerAState = playerAState;
            PlayerBState = playerBState;
        }

        public static Game NewGame(string playerA, string playerB) => new Game(playerA, playerB);

        public void PlayerAPlaceShip(Ship ship)
        {
            PlayerAState = PlaceShip(ship, PlayerAState, BoardA);
            IsGameReadyToStart();
        }

        public void PlayerATakeShot(Shot shot)
        {
            PlayerAState = TakeShot(shot, PlayerAState, BoardB);
            PlayerBState = PlayerAState == GameState.Win ? GameState.Lose : GameState.Fire;
        }

        public void PlayerBPlaceShip(Ship ship)
        {
            PlayerBState = PlaceShip(ship, PlayerBState, BoardB);
            IsGameReadyToStart();
        }

        public void PlayerBTakeShot(Shot shot)
        {
            PlayerBState = TakeShot(shot, PlayerBState, BoardA);
            PlayerAState = PlayerBState == GameState.Win ? GameState.Lose : GameState.Fire;
        }

        private static GameState PlaceShip(Ship ship, GameState state, Board board)
        {
            void PlaceShipHelper(int length)
            {
                if (ship.Length != length)
                    throw new ShipBattlesIllegalMoveException($"Must place ship of length {length}");
                board.PlaceShip(ship);
            }

            switch (state)
            {
                case GameState.PlaceShip4:
                    PlaceShipHelper(4);
                    return GameState.PlaceShip3;
                case GameState.PlaceShip3:
                    PlaceShipHelper(3);
                    return GameState.PlaceShip2;
                case GameState.PlaceShip2:
                    PlaceShipHelper(2);
                    return GameState.PlaceShip1;
                case GameState.PlaceShip1:
                    PlaceShipHelper(1);
                    return GameState.ReadyToPlay;

                case GameState.Fire:
                    throw new ShipBattlesIllegalMoveException("Cannot fire at this point");

                case GameState.NotYourTurn:
                    throw new ShipBattlesOutOfTurnException();

                case GameState.Win:
                case GameState.Lose:
                    throw new ShipBattlesGameOverException();

                default:
                    throw new System.NotImplementedException();
            }
        }

        private static GameState TakeShot(Shot shot, GameState state, Board board)
        {
            switch (state)
            {
                case GameState.Fire:
                    board.Fire(shot);
                    return board.AreAllShipsSunk() ? GameState.Win : GameState.NotYourTurn;

                case GameState.PlaceShip1:
                case GameState.PlaceShip2:
                case GameState.PlaceShip3:
                case GameState.PlaceShip4:
                    throw new ShipBattlesIllegalMoveException("Cannot place ship at this point");

                case GameState.NotYourTurn:
                    throw new ShipBattlesOutOfTurnException();

                case GameState.Win:
                case GameState.Lose:
                    throw new ShipBattlesGameOverException();

                default:
                    throw new System.NotImplementedException();
            }
        }

        private void IsGameReadyToStart()
        {
            if (PlayerAState == GameState.ReadyToPlay && PlayerBState == GameState.ReadyToPlay)
            {
                PlayerAState = GameState.NotYourTurn;
                PlayerBState = GameState.Fire;
            }
        }

    }
}