using System;

namespace se.Urbaino.ShipBattles.Web.Hubs.Lobby
{
    public class GameDTO
    {
        public string Id { get; set; }
        public string OpponentName { get; set; }
        public bool? ResultIsVictory { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (GameDTO)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}