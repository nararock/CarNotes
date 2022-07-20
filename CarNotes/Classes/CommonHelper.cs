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
        /// возвращает список событий для общей таблицы
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public List<CommonModel> GetList(int vehicleId, int pageNumder, int pageSize)
        {
            var db = new CnDbContext();
            var vehicle = db.Vehicles.Find(vehicleId);
            if (vehicle == null) return null;
            SqlParameter paramId = new SqlParameter("@Id", vehicleId);
            SqlParameter paramAmountOffset = new SqlParameter("@amountOffset", (pageNumder - 1) * pageSize);
            SqlParameter paramAmountGet = new SqlParameter("@amountGet", pageSize);
            var commonModel = db.Database.SqlQuery<CommonModel>(@"select top(@amountGet) * from
                                                       (select Id, Date, Mileage, Cost, Record, WrongMileage from 
                                                          (select ID as Id, Date, 1 as Record, Mileage, ROUND(PricePerOneLiter*Volume, 0) as Cost, WrongMileage from RefuelEvents Where VehicleId = @Id
                                                           union 
                                                           (select Id, Date, 2 as Record, Mileage, ROUND((Cast(RepairCost as float) + ISNULL(Price,0)), 0) as Cost, WrongMileage from RepairEvents as re
                                                           left outer join (select RepairEvent_Id, SUM(Price) as price from CarParts group by RepairEvent_Id) as t on re.Id = t.RepairEvent_Id
                                                           Where VehicleId = @Id)
                                                           union  
                                                           select Id, Date, 3 as Record, Mileage, ROUND(Cast(Sum as float), 0) as Cost, WrongMileage from Expenses Where VehicleId = @Id) as d
                                                           order by Date desc, Mileage desc
                                                           offset @amountOffset rows) as data1", paramId, paramAmountOffset, paramAmountGet).ToList();
            return commonModel;
        }

        public double? GetLastMileage(int vehicleId)
        {
            var db = new CnDbContext();
            db.Database.Log += s => System.Diagnostics.Debug.WriteLine(s);
            SqlParameter param = new SqlParameter("@Id", vehicleId);
            var maxMileage = db.Database.SqlQuery<Mileage>(@"Select MAX(Mileage) as LastMileage from(Select * from (SELECT top(1) Mileage from RefuelEvents where VehicleId = @Id order by Mileage desc) as t1
                                                                                                    union
                                                                                                    Select * from(SELECT top(1) Mileage from RepairEvents where VehicleId = @Id order by Mileage desc) as t2) 
                                                                                                    union
				                                                                                    Select * from(SELECT top(1) Mileage from Expenses where VehicleId = 3 order by Mileage desc) as t3) as t", param).ToList();
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