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
            var list = vehicle.RefuelEvents.Select(x => new CommonModel { Id = x.ID, Record = Enums.RecordType.Refuel, Date = x.Date.ToString("dd.MM.yyyy"), Mileage = x.Mileage, Cost = Math.Round(x.PricePerOneLiter * x.Volume) }).ToList();
            list.AddRange(vehicle.RepairEvents.Select(x => new CommonModel { Id = x.Id, Record = Enums.RecordType.Repair, Date = x.Date.ToString("dd.MM.yyyy"), Mileage = x.Mileage, Cost = (double)x.RepairCost }));
            list.AddRange(vehicle.Expenses.Select(x => new CommonModel { Id = x.Id, Record = Enums.RecordType.Expense, Date = x.Date.ToString("dd.MM.yyyy"), Mileage = x.Mileage ?? 0, Cost = (double)x.Sum }));
            list = list.OrderByDescending(x => x.Mileage).ToList();
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
        /// <summary>
        /// проверка соответсвия между UserId пользователя и vehicleId автомобиля
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public bool GetAccessToVehicle(string UserId, int vehicleId, CnDbContext db)
        {
            var vehicle = db.Vehicles.Find(vehicleId);
            if (vehicle == null)
            {
                return false;
            }
            return vehicle.UserId == UserId;
        }

        /// <summary>
        /// проверка корректности введенного пробега (больше предыдущего, меньше последующего)
        /// </summary>
        /// <param name="date"></param> дата проверяемого события
        /// <param name="mileage"></param> пробег проверяемого события
        /// <param name="vehicleId"></param> Id автомобиля
        /// <returns></returns>
        public bool CheckMileage(string date, string mileage, int vehicleId)
        {
            var db = new CnDbContext();
            var dateEvent = DateTime.Parse(date);
            var mileageEvent = double.Parse(mileage);
            var earlyEvent = db.RefuelEvents
                  .Where(x => x.VehicleId == vehicleId && x.Date < dateEvent)
                  .Select(x => new { date = x.Date, mileage = x.Mileage })
                  .OrderByDescending(x => x.date)
                  .ThenByDescending(x => x.mileage)
                  .Take(1)
                  .Union(db.RepairEvents  
                      .Where(x => x.VehicleId == vehicleId && x.Date < dateEvent)
                      .Select(x => new { date = x.Date, mileage = x.Mileage })
                      .OrderByDescending(x => x.date)
                      .ThenByDescending(x => x.mileage)
                      .Take(1))
                  .Union(db.Expenses
                      .Where(x => x.VehicleId == vehicleId && x.Date < dateEvent && x.Mileage != null)
                      .Select(x => new { date = x.Date, mileage = (double)x.Mileage })
                      .OrderByDescending(x => x.date)
                      .ThenByDescending(x => x.mileage)
                      .Take(1))
                  .OrderByDescending(x => x.date)
                  .ThenByDescending(x => x.mileage)
                  .FirstOrDefault();
            
            var lateEvent = db.RefuelEvents
                  .Where(x => x.VehicleId == vehicleId && x.Date > dateEvent)
                  .Select(x => new { date = x.Date, mileage = x.Mileage })
                  .OrderBy(x => x.date)
                  .ThenBy(x => x.mileage)
                  .Take(1)
                  .Union(db.RepairEvents
                      .Where(x => x.VehicleId == vehicleId && x.Date > dateEvent)
                      .Select(x => new { date = x.Date, mileage = x.Mileage })
                      .OrderBy(x => x.date)
                      .ThenBy(x => x.mileage)
                      .Take(1))
                  .Union(db.Expenses
                      .Where(x => x.VehicleId == vehicleId && x.Date > dateEvent && x.Mileage != null)
                      .Select(x => new { date = x.Date, mileage = (double)x.Mileage })
                      .OrderByDescending(x => x.date)
                      .ThenByDescending(x => x.mileage)
                      .Take(1))
                  .OrderBy(x => x.date)
                  .ThenBy(x => x.mileage)
                  .FirstOrDefault();

            var startMileage = earlyEvent == null ? int.MinValue : earlyEvent.mileage;
            var endMileage = lateEvent == null ? int.MaxValue : lateEvent.mileage;

            if (mileageEvent > startMileage && mileageEvent < endMileage)
            {
                return true;
            }
            else { return false; }
        }
    }
}