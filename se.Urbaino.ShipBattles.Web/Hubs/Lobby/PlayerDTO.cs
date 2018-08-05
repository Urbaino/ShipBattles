namespace se.Urbaino.ShipBattles.Web.Hubs.Lobby
{
    public class PlayerDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            var other = (PlayerDTO)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}