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
    }
}
