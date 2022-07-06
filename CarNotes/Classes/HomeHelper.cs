using CarNotes.CnDb;
using CarNotes.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CarNotes.Classes
{
    public class HomeHelper
    {
        public List<LastEventModel> GetLastEvents()
        {
            var db = new CnDbContext();
            var dateCompare = DateTime.Now.AddDays(1);
            var list = db.RefuelEvents.Include(v => v.Vehicle)
                .Where(x=>x.Date <= dateCompare)
                .Select(x => new LastEventModel
            {
                Id = x.VehicleId,
                VehicleBrand = x.Vehicle.Brand,
                VehicleModel = x.Vehicle.Model,
                Record = Enums.RecordType.Refuel,
                Date = x.Date,
                Cost = (int)(x.PricePerOneLiter * x.Volume)
            }).OrderByDescending(x => x.Date).Take(10).ToList();
            list.AddRange(db.RepairEvents.Include(v => v.Vehicle).Include(c => c.Parts)
                .Where(x => x.Date <= dateCompare)
                .Select(x => new LastEventModel
                {
                    Id = x.VehicleId,
                    VehicleBrand = x.Vehicle.Brand,
                    VehicleModel = x.Vehicle.Model,
                    Record = Enums.RecordType.Repair,
                    Date = x.Date,
                    Cost = (int)x.RepairCost + (int)x.Parts.Sum(c => c.Price)
                }).OrderByDescending(x => x.Date).Take(10)) ;
            list.AddRange(db.Expenses.Include(v => v.Vehicle)
                .Where(x => x.Date <= dateCompare)
                .Select(x => new LastEventModel
            {
                Id = x.VehicleId,
                VehicleBrand = x.Vehicle.Brand,
                VehicleModel = x.Vehicle.Model,
                Record = Enums.RecordType.Expense,
                Date = x.Date,
                Cost = (int)x.Sum
            }).OrderByDescending(x => x.Date).Take(10));
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

        public List<ActiveUsersModel> GetActiveUsers()
        {
            var db = new CnDbContext();
            db.Database.Log += s => System.Diagnostics.Debug.WriteLine(s);
            var activeUsers = db.Database.SqlQuery<ActiveUsersModel>(@"select top(10) u.Name,v.Id as vehicleID, v.Brand, v.Model,isnull(table1.Events,0) as refuelEvents,isnull(table2.Events,0) as repairEvents, isnull(table1.Events,0)+isnull(table2.Events,0) as events from AspNetUsers as u
                                                    join Vehicles as v on u.Id = v.UserId
                                                    left join (select VehicleId, COUNT(ID) as Events from RefuelEvents
	                                                where Date > DATEADD(day, -30, GETDATE())
	                                                group by VehicleId) as table1 on v.Id = table1.VehicleId
                                                    left join (select VehicleId, COUNT(ID) as Events from RepairEvents
	                                                where Date > DATEADD(day, -30, GETDATE())
	                                                group by VehicleId) as table2 on v.Id = table2.VehicleId
                                                    group by u.Id, u.Name, v.Id, v.Brand, v.Model,isnull(table1.Events,0), isnull(table2.Events,0), isnull(table1.Events,0)+isnull(table2.Events,0)
                                                    order by isnull(table1.Events,0)+isnull(table2.Events,0) desc").ToList();
            return activeUsers;
        } 

        public int GetAmountVehicles()
        {
            var db = new CnDbContext();
            var result = db.Vehicles.Count();
            return result;
        }

        public int GetAmountUsers()
        {
            var db = new CnDbContext();
            var result = db.Users.Count();
            return result;
        }

        public int GetAmountRepairEvents()
        {
            var db = new CnDbContext();
            var result = db.RepairEvents.Count();
            return result;
        }

        public int GetAmountRefuelEvents()
        {
            var db = new CnDbContext();
            var result = db.RefuelEvents.Count();
            return result;
        }
    }
}