﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CarNotes.CnDb
{
    public class CnDbContext : IdentityDbContext<User>
    {
        public CnDbContext() : base("name=CnDb") { }
        public DbSet<RefuelEvent> RefuelEvents { get; set; }
        public DbSet<GasStation> GasStations { get; set; }
        public DbSet<RepairEvent> RepairEvents { get; set; }
        public DbSet<CarPart> CarParts { get; set; }
        public DbSet<Vehicle> Vehicles  {get; set;}
        public DbSet<CarSystem> CarSystems { get; set; }
        public DbSet<CarSubsystem> CarSubsystems { get; set; }
        /// <summary> Типы расходов </summary>
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        /// <summary> Расходы </summary>
        public DbSet<Expense> Expenses { get; set; }

        public static CnDbContext Create()
        {
            return new CnDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GasStation>().HasKey(g => g.ID);
            modelBuilder.Entity<Vehicle>().HasRequired(v => v.User).WithMany(u => u.Vehicles).HasForeignKey(v => v.UserId);
            modelBuilder.Entity<RefuelEvent>().HasRequired(r => r.Vehicle).WithMany(v => v.RefuelEvents).HasForeignKey(r => r.VehicleId).WillCascadeOnDelete(true);
            modelBuilder.Entity<RepairEvent>().HasRequired(r => r.Vehicle).WithMany(v => v.RepairEvents).HasForeignKey(r => r.VehicleId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Expense>().HasRequired(r => r.Vehicle).WithMany(v => v.Expenses).HasForeignKey(r => r.VehicleId).WillCascadeOnDelete(true);
            modelBuilder.Entity<RefuelEvent>().HasRequired(r => r.Station).WithMany().HasForeignKey(r => r.Station_ID);
            modelBuilder.Entity<CarSystem>().HasMany(r => r.Subsystems).WithRequired(r=>r.CarSystem).HasForeignKey(r=>r.CarsystemId);
            modelBuilder.Entity<CarSubsystem>().HasMany(r => r.CarParts).WithRequired(r => r.CarSubsystem).HasForeignKey(r => r.CarSubsystemId);
            modelBuilder.Entity<RepairEvent>().HasMany(r => r.Parts).WithRequired().HasForeignKey(r => r.RepairEventId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Expense>().HasRequired(e => e.Type).WithMany().HasForeignKey(e => e.TypeId);
            base.OnModelCreating(modelBuilder);
        }
    }
}