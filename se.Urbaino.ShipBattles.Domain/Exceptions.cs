using System;

namespace se.Urbaino.ShipBattles.Domain.Exceptions
{
    public class ShipBattlesException : Exception
    {
        public ShipBattlesException(string message) : base(message) { }
    }

    public class ShipBattlesOutOfTurnException : ShipBattlesException
    {
        public ShipBattlesOutOfTurnException() : base("Out of turn") { }
    }
    public class ShipBattlesIllegalMoveException : ShipBattlesException
    {
        public ShipBattlesIllegalMoveException(string message) : base(message) { }
    }
    public class ShipBattlesGameOverException : ShipBattlesException
    {
        public ShipBattlesGameOverException() : base("Game is over") { }
    }
}