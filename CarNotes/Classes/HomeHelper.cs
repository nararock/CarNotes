using CarNotes.CnDb;
using CarNotes.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CarNotes.Classes
{
    public class HomeHelper
    {
        public List<LastEventModel> GetLastEvents()
        {
            var db = new CnDbContext();
            var list = db.RefuelEvents.Include(v => v.Vehicle).Select(x => new LastEventModel
            {
                Id = x.VehicleId,
                VehicleBrand = x.Vehicle.Brand,
                VehicleModel = x.Vehicle.Model,
                Record = Enums.RecordType.Refuel,
                Date = x.Date,
                Cost = (int)(x.PricePerOneLiter * x.Volume)
            }).OrderBy(x => x.Date).Take(10).ToList();
            list.AddRange(db.RepairEvents.Include(v => v.Vehicle).Select(x => new LastEventModel
            {
                Id = x.VehicleId,
                VehicleBrand = x.Vehicle.Brand,
                VehicleModel = x.Vehicle.Model,
                Record = Enums.RecordType.Repair,
                Date = x.Date,
                Cost = (int)x.RepairCost
            }).OrderBy(x => x.Date).Take(10));
            list.AddRange(db.Expenses.Include(v => v.Vehicle).Select(x => new LastEventModel
            {
                Id = x.VehicleId,
                VehicleBrand = x.Vehicle.Brand,
                VehicleModel = x.Vehicle.Model,
                Record = Enums.RecordType.Expense,
                Date = x.Date,
                Cost = (int)x.Sum
            }).OrderBy(x => x.Date).Take(10));
            list = list.OrderByDescending(x => x.Date).Take(10).ToList();
            return list;
        }

        public List<string> GetLastVisit(string userId)
        {
            var db = new CnDbContext();
            var result = new List<string>();
            var userDb = db.Users.FirstOrDefault(x => x.Id == userId);
            if (userDb != null)
            {
                result.Add(userDb.LastVisit.ToString("dd.MM.yyyy"));
                result.Add(userDb.LastVisit.ToString("HH:mm:ss"));
                return result;
            }
            return result;
        }
    }
}