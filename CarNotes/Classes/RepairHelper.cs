using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;

namespace CarNotes.Classes
{
    public class RepairHelper
    {
        public List<RepairModel> GetList(int vehicleId, int pageNumder, int pageSize)
        {
            var db = new CnDbContext();
            var vehicle = db.Vehicles.Find(vehicleId);
            if (vehicle == null) return null;
            SqlParameter paramId = new SqlParameter("@Id", vehicleId);
            SqlParameter paramAmountOffset = new SqlParameter("@amountOffset", (pageNumder - 1) * pageSize);
            SqlParameter paramAmountGet = new SqlParameter("@amountGet", pageSize);
            var repairModel = db.Database.SqlQuery<RepairModel>(@"select top(@amountGet) * from(select Id, Date, Mileage, Repair, ROUND((Cast(RepairCost as float) + ISNULL(Price,0)), 0) as RepairCost, CarService, Comments, WrongMileage from RepairEvents as re
                                                                left outer join (select RepairEvent_Id, SUM(Price) as price from CarParts group by RepairEvent_Id) as t on re.Id = t.RepairEvent_Id
                                                                Where VehicleId = @Id
                                                                order by Date desc, Mileage desc
                                                                offset @amountOffset rows) as data", paramId, paramAmountOffset, paramAmountGet).ToList();
            return repairModel;
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

        public RepairModel GetDataEdit(int id, int? vehicleId)
        {
            var db = new CnDbContext();
            var editRepair = db.RepairEvents
                .Include(x => x.Parts)
                .Include(x => x.Parts.Select(p => p.CarSubsystem))
                .FirstOrDefault(y => y.Id == id);
            
            var editRepairModel = new RepairModel();            
            
            if (editRepair == null)
            {
                editRepairModel.Mileage = new CommonHelper().GetLastMileage(vehicleId.GetValueOrDefault(0)).GetValueOrDefault(0);
                return editRepairModel;
            }
            editRepairModel.Id = editRepair.Id;
            editRepairModel.Mileage = editRepair.Mileage;
            editRepairModel.Repair = editRepair.Repair;
            editRepairModel.RepairCost = (int)editRepair.RepairCost;
            editRepairModel.Date = editRepair.Date;
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

        public void ChangeData(RepairModel rm, int vehicleId, HttpContextBase hc)
        {
            var db = new CnDbContext();
            var checkUser = new CommonHelper().GetAccessToVehicle(hc.User.Identity.GetUserId(), vehicleId, db);
            if (!checkUser) { return; }
            var repairEvent = db.RepairEvents
                .Include(x => x.Parts)
                .Include(x => x.Parts.Select(p => p.CarSubsystem))
                .Where(y => y.Id == rm.Id)
                .FirstOrDefault();
            if (repairEvent == null)
            {
                repairEvent = new RepairEvent();
                repairEvent.Parts = new List<CarPart>();
                db.RepairEvents.Add(repairEvent);
                repairEvent.VehicleId = vehicleId;
            }
            repairEvent.CarService = rm.CarService;
            repairEvent.Comments = rm.Comments;
            repairEvent.Date = rm.Date; ;
            repairEvent.Mileage = rm.Mileage;
            if (new CommonHelper().CheckMileage(rm.Date, Convert.ToString(rm.Mileage), vehicleId))
            {
                repairEvent.WrongMileage = false;
            } else { repairEvent.WrongMileage = true;}
            repairEvent.Repair = rm.Repair;
            repairEvent.RepairCost = (Decimal)rm.RepairCost;
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