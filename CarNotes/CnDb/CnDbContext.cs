using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CarNotes.CnDb
{
    public class CnDbContext: DbContext
    {
        public DbSet<RefuelEvent> RefuelEvents { get; set; }
        public DbSet<GasStation> GasStations { get; set; }
        public DbSet<RepairEvent> RepairEvents { get; set; }
        public DbSet<CarPart> CarParts { get; set; }
        public CnDbContext() : base("name=CnDb") { }
    }
}