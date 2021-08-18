using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity;

namespace CarNotes.Classes
{
    public class RepairHelper
    {
        public List<RepairModel> GetList(int vehicleId)
        {
            var db = new CnDbContext();
            var vehicle = db.Vehicles.Include(v=>v.RepairEvents.Select(r=>r.Parts)).FirstOrDefault(x => x.Id == vehicleId);
            if (vehicle == null) return null;
            var list = vehicle.RepairEvents.Select(x => new RepairModel {Id = x.Id, Date=x.Date.ToString(), Mileage=x.Mileage, Repair=x.Repair,
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
            var tempDate = new DateTime();
            DateTime.TryParse(rm.Date, out tempDate);
            repairEvent.Date = tempDate;
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

        public void Delete(int id, HttpContextBase hc)
        {
            var data = new CnDbContext();
            var repair = data.RepairEvents.Include(x=>x.Parts).FirstOrDefault(x => x.Id == id);
            if (repair?.Vehicle?.UserId == hc.User.Identity.GetUserId())
            {
                var parts = repair.Parts;
                for(int i = 0; i < parts.Count;)
                {
                    parts.RemoveAt(i);
                }
                data.RepairEvents.Remove(repair);
                data.SaveChanges();
            }
        }

        public RepairModel GetDataEdit(int id)
        {
            var db = new CnDbContext();
            var editRepair = db.RepairEvents.Include(x => x.Parts).FirstOrDefault(y => y.Id == id);
            if (editRepair == null)
            {
                return new RepairModel();
            }
            var editRepairModel = new RepairModel();            
            editRepairModel.Id = editRepair.Id;
            editRepairModel.Mileage = editRepair.Mileage;
            editRepairModel.Repair = editRepair.Repair;
            editRepairModel.RepairCost = editRepair.RepairCost;
            editRepairModel.Date = editRepair.Date.ToString("dd.MM.yyyy");
            editRepairModel.CarService = editRepair.CarService;
            editRepairModel.Comments = editRepair.Comments;
            editRepairModel.Parts = new List<CarPartModel>();
            for (int i = 0; i < editRepair.Parts.Count; i++)
            {
                var editCarPartModel = new CarPartModel();
                editCarPartModel.Article = editRepair.Parts[i].Article;
                editCarPartModel.CarManufacturer = editRepair.Parts[i].CarManufacturer;
                editCarPartModel.Name = editRepair.Parts[i].Name;
                editCarPartModel.Price = editRepair.Parts[i].Price;
                editRepairModel.Parts.Add(editCarPartModel);
            }
            return editRepairModel;
        }
    }
}