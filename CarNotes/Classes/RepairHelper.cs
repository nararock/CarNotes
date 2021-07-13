using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace CarNotes.Classes
{
    public class RepairHelper
    {
        public List<RepairModel> GetList(int vehicleId)
        {
            var db = new CnDbContext();
            var vehicle = db.Vehicles.Include(v=>v.RepairEvents.Select(r=>r.Parts)).FirstOrDefault(x => x.Id == vehicleId);
            if (vehicle == null) return null;
            var list = vehicle.RepairEvents.Select(x => new RepairModel { Date=x.Date, Mileage=x.Mileage, Repair=x.Repair,
                CarService=x.CarService, RepairCost=x.RepairCost, Comments=x.Comments, Parts=x.Parts.Select(y=>new CarPartModel { Article=y.Article,
                    CarManufacturer=y.CarManufacturer, Name=y.Name, Price=y.Price}).ToList() }).ToList();
            return list;
        }
        public void SaveToDataBase(RepairModel rm, int vehicleId)
        {
            var database = new CnDbContext();
            var repairEvent = new RepairEvent();
            repairEvent.CarService = rm.CarService;
            repairEvent.Comments = rm.Comments;
            repairEvent.Date = rm.Date;
            repairEvent.Mileage = rm.Mileage;
            repairEvent.Repair = rm.Repair;
            repairEvent.RepairCost = rm.RepairCost;
            repairEvent.VehicleId = vehicleId;
            repairEvent.Parts = new List<CarPart>();
            for (int i = 0; i < rm.Parts.Count; i++)
            {
                var carPart = new CarPart();
                carPart.Article = rm.Parts[i].Article;
                carPart.CarManufacturer = rm.Parts[i].CarManufacturer;
                carPart.Name = rm.Parts[i].Name;
                carPart.Price = rm.Parts[i].Price;
                repairEvent.Parts.Add(carPart);
            }
            database.RepairEvents.Add(repairEvent);
            database.SaveChanges();
        }
    }
}