using System;
using Microsoft.EntityFrameworkCore;
using se.Urbaino.ShipBattles.Domain.Games;

namespace se.Urbaino.ShipBattles.Data
{
    public class ShipBattlesContext : DbContext
    {
        public ShipBattlesContext(DbContextOptions<ShipBattlesContext> options) : base(options) { }

        public DbSet<Game> Games { get; private set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Game>(e =>
            {
                e.HasKey(g => g.Id);
                e.OwnsOne(g => g.BoardA);
                e.OwnsOne(g => g.BoardB);
            });
        }
    }
}
