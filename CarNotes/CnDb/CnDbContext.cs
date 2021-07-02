using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CarNotes.CnDb
{
    public class CnDbContext : IdentityDbContext<User>
    {
        public DbSet<RefuelEvent> RefuelEvents { get; set; }
        public DbSet<GasStation> GasStations { get; set; }
        public DbSet<RepairEvent> RepairEvents { get; set; }
        public DbSet<CarPart> CarParts { get; set; }
        public DbSet<Vehicle> Vehicles  {get; set;}
        public CnDbContext() : base("name=CnDb") { }
        public static CnDbContext Create()
        {
            return new CnDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().HasRequired(v => v.User).WithMany(u => u.Vehicles).HasForeignKey(v => v.UserId);
            modelBuilder.Entity<RefuelEvent>().HasRequired(r => r.Vehicle).WithMany(v => v.RefuelEvents).HasForeignKey(r => r.VehicleId);
            modelBuilder.Entity<RepairEvent>().HasRequired(r => r.Vehicle).WithMany(v => v.RepairEvents).HasForeignKey(r => r.VehicleId);

            base.OnModelCreating(modelBuilder);
        }
    }
}