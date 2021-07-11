using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Classes
{
    public class CommonHelper
    {
        public List<CommonModel> CreateList(int vehicleId)
        {
            var db = new CnDbContext();
            var vehicle = db.Vehicles.Find(vehicleId);
            if (vehicle == null) return null;
            var list = vehicle.RefuelEvents.Select(x => new CommonModel {Record = Enums.RecordType.Refuel, Date = x.Date, Mileage = x.Mileage, Cost = x.PricePerOneLiter * x.Volume }).ToList();
            list.AddRange(vehicle.RepairEvents.Select(x => new CommonModel {Record = Enums.RecordType.Repair, Date = x.Date, Mileage = x.Mileage, Cost = (double)x.RepairCost }));
            list = list.OrderBy(x => x.Mileage).ToList();
            return list;
        }
    }
}