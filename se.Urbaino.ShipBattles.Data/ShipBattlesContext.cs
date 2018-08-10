using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using se.Urbaino.ShipBattles.Data.DBO;

namespace se.Urbaino.ShipBattles.Data
{
    public class ShipBattlesContext : DbContext
    {
        public ShipBattlesContext(DbContextOptions<ShipBattlesContext> options) : base(options) { }

        internal DbSet<GameDBO> Games { get; private set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // TODO: Detta pajjar ChangeTrackern
            ValueConverter<T, string> JsonValueConverter<T>()
            {
                return new ValueConverter<T, string>(
                    b => JsonConvert.SerializeObject(b),
                    s => JsonConvert.DeserializeObject<T>(s)
                );
            }

            // builder.Entity<GameDBO>(e =>
            // {
            //     e.Property(g => g.ShipsA).HasConversion(JsonValueConverter<List<ShipDBO>>());
            //     e.Property(g => g.ShipsB).HasConversion(JsonValueConverter<List<ShipDBO>>());
            // });

            // builder.Entity<ShipDBO>(e =>
            // {
            // });

            // builder.Entity<ShotDBO>(e =>
            // {
            // });
        }

    }
}
