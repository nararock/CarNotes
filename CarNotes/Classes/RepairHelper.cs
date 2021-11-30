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
            var list = vehicle.RepairEvents.Select(x => new RepairModel {Id = x.Id, Date=x.Date.ToString("dd.MM.yyyy"), Mileage=x.Mileage, Repair=x.Repair,
                CarService=x.CarService, RepairCost=x.RepairCost, Comments=x.Comments, Parts=x.Parts.Select(y=>new CarPartModel { Article=y.Article,
                    CarManufacturer=y.CarManufacturer, Name=y.Name, Price=y.Price}).ToList() }).ToList();
            return list;
        }
        
        public void Delete(int id, HttpContextBase hc)
        {
            var data = new CnDbContext();
            var repair = data.RepairEvents.Include(x=>x.Parts).FirstOrDefault(x => x.Id == id);
            if (repair?.Vehicle?.UserId == hc.User.Identity.GetUserId())
            {
                data.RepairEvents.Remove(repair);
                data.SaveChanges();
            }
        }

        public RepairModel GetDataEdit(int id)
        {
            var db = new CnDbContext();
            var editRepair = db.RepairEvents
                .Include(x => x.Parts)
                .Include(x => x.Parts.Select(p => p.CarSubsystem))
                .FirstOrDefault(y => y.Id == id);
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
                editCarPartModel.Id = editRepair.Parts[i].Id;
                editCarPartModel.Article = editRepair.Parts[i].Article;
                editCarPartModel.CarManufacturer = editRepair.Parts[i].CarManufacturer;
                editCarPartModel.Name = editRepair.Parts[i].Name;
                editCarPartModel.Price = editRepair.Parts[i].Price;
                editCarPartModel.SystemId = editRepair.Parts[i].CarSubsystem.CarsystemId;
                editCarPartModel.SubSystemId = editRepair.Parts[i].CarSubsystemId;
                editRepairModel.Parts.Add(editCarPartModel);
            }
            return editRepairModel;
        }

        public void ChangeData(RepairModel rm, int vehicleId)
        {
            var db = new CnDbContext();
            
            var repairEvent = db.RepairEvents
                .Include(x => x.Parts)
                .Include(x => x.Parts.Select(p => p.CarSubsystem))
                .Where(y => y.Id == rm.Id)
                .FirstOrDefault();
            if (repairEvent == null)
            {
                repairEvent = new RepairEvent();
                db.RepairEvents.Add(repairEvent);
                repairEvent.VehicleId = vehicleId;
            }
            repairEvent.CarService = rm.CarService;
            repairEvent.Comments = rm.Comments;
            var tempDate = new DateTime();
            DateTime.TryParse(rm.Date, out tempDate);
            repairEvent.Date = tempDate;
            repairEvent.Mileage = rm.Mileage;
            repairEvent.Repair = rm.Repair;
            repairEvent.RepairCost = rm.RepairCost;
            var carPartsDelete = new List<CarPart>();
            foreach(var p in rm.Parts)
            {
                if (p.Id != 0 && p.IsDeleted == true)
                {
                    db.CarParts.Remove(repairEvent.Parts.Where(x => x.Id == p.Id).FirstOrDefault());
                } 
                else if (p.Id != 0)
                {
                    var carPartDb = repairEvent.Parts.Where(x => x.Id == p.Id).FirstOrDefault();
                    carPartDb.Article = p.Article;
                    carPartDb.CarManufacturer = p.CarManufacturer;
                    carPartDb.Name = p.Name;
                    carPartDb.Price = p.Price;
                    carPartDb.CarSubsystemId = p.SubSystemId;
                } 
                else if (p.Id == 0 && p.IsDeleted != true)
                {
                    var carParts = new CarPart();
                    carParts.Name = p.Name;
                    carParts.Price = p.Price;
                    carParts.Article = p.Article;
                    carParts.CarManufacturer = p.CarManufacturer;
                    carParts.CarSubsystemId = p.SubSystemId;
                    repairEvent.Parts.Add(carParts);
                }
            }
            db.SaveChanges();
        }
        
       public List<CarSystemModel> GetSystemList()
        {
            var db = new CnDbContext();
            var listSystem = db.CarSystems.Include(c => c.Subsystems).ToList()
                .Select(cs => new CarSystemModel
                {
                    Name = cs.Name,
                    Id = cs.Id,
                    CarSubsystems = cs.Subsystems.Select(ss => new CarSubsystemModel { Name = ss.Name, Id = ss.Id }).ToList()
                }).ToList();
            return listSystem;
        }
    }
}