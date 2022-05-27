using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace CarNotes.Classes
{
    public class CommonHelper
    {
        /// <summary>
        /// возвращает список общих событий
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public List<CommonModel> CreateList(int vehicleId)
        {
            var db = new CnDbContext();
            var vehicle = db.Vehicles.Find(vehicleId);
            if (vehicle == null) return null;
            var list = vehicle.RefuelEvents.Select(x => new CommonModel {Id = x.ID, Record = Enums.RecordType.Refuel, Date = x.Date.ToString("dd.MM.yyyy"), Mileage = x.Mileage, Cost = Math.Round(x.PricePerOneLiter * x.Volume) }).ToList();
            list.AddRange(vehicle.RepairEvents.Select(x => new CommonModel {Id = x.Id, Record = Enums.RecordType.Repair, Date = x.Date.ToString("dd.MM.yyyy"), Mileage = x.Mileage, Cost = (double)x.RepairCost }));
            list.AddRange(vehicle.Expenses.Select(x => new CommonModel { Id = x.Id, Record = Enums.RecordType.Expense, Date = x.Date.ToString("dd.MM.yyyy"), Mileage = x.Mileage??0, Cost = (double)x.Sum }));
            list = list.OrderBy(x => x.Mileage).ToList();
            return list;
        }

        public double? GetLastMileage(int vehicleId)
        {
            var db = new CnDbContext();
            db.Database.Log += s => System.Diagnostics.Debug.WriteLine(s);
            SqlParameter param = new SqlParameter("@Id", vehicleId);
            var maxMileage = db.Database.SqlQuery<Mileage>(@"Select MAX(Mileage) as LastMileage from(Select * from (SELECT top(1) Mileage from RefuelEvents where VehicleId = @Id order by Mileage desc) as t1
                union
                Select * from(SELECT top(1) Mileage from RepairEvents where VehicleId = @Id order by Mileage desc) as t2) as t", param).ToList();
            if (maxMileage[0].LastMileage == null)
            {
               return 0;
            }
            return maxMileage[0].LastMileage;
        }
    }
}