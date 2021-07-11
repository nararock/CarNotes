using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Classes
{
    public class RepairHelper
    {
        public void SaveToDataBase(RepairModel rm, int vehicleId)
        {
            var database = new CnDbContext();
            var repairEvent = new RepairEvent();
            repairEvent.CarService = rm.CarService;
            repairEvent.Comments = rm.Comments;
            repairEvent.Date = DateTime.ParseExact(rm.Date, "dd.MM.yyyy", null);
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