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
                VehicleBrand = x.Vehicle.Brand,
                VehicleModel = x.Vehicle.Model,
                Record = Enums.RecordType.Refuel,
                Date = x.Date,
                Cost = (x.PricePerOneLiter * x.Volume).ToString()
            }).OrderBy(x => x.Date).Take(10).ToList();
            list.AddRange(db.RepairEvents.Include(v => v.Vehicle).Select(x => new LastEventModel
            {
                VehicleBrand = x.Vehicle.Brand,
                VehicleModel = x.Vehicle.Model,
                Record = Enums.RecordType.Repair,
                Date = x.Date,
                Cost = x.RepairCost.ToString()
            }).OrderBy(x => x.Date).Take(10));
            list.AddRange(db.Expenses.Include(v => v.Vehicle).Select(x => new LastEventModel
            {
                VehicleBrand = x.Vehicle.Brand,
                VehicleModel = x.Vehicle.Model,
                Record = Enums.RecordType.Expense,
                Date = x.Date,
                Cost = x.Sum.ToString()
            }).OrderBy(x => x.Date).Take(10));
            list.OrderBy(x => x.Date).Take(6);
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