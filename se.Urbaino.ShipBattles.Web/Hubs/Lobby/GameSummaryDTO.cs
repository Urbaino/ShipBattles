using System;

namespace se.Urbaino.ShipBattles.Web.Hubs.Lobby
{
    public class GameSummaryDTO
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

            var other = (GameSummaryDTO)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}