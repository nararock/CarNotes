using CarNotes.CnDb;
using CarNotes.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Classes
{
    public class VehicleHelper
    {
        public List<VehicleModel> GetVehicles(HttpContextBase context)
        {
            var database = new CnDbContext();
            var userId = new AuthHelper(context).AuthenticationManager.User.Identity.GetUserId();
            var answer = database.Vehicles.Where(x => x.UserId == userId).Select(x => new VehicleModel
            {
                Id = x.Id,
                Brand = x.Brand,
                Body = x.Body,
                ReleaseYear = x.ReleaseYear,
                Color = x.Color,
                Model = x.Model
            }).ToList();
            return answer;
        }

        public List<VehicleModel> GetVehicles(int? vehicleId)
        {
            if (vehicleId == null) return new List<VehicleModel>();
            var db = new CnDbContext();
            var userId = db.Vehicles.Find(vehicleId).UserId;
            var answer = db.Vehicles.Where(x => x.UserId == userId).Select(x => new VehicleModel
            {
                Id = x.Id,
                Brand = x.Brand,
                Body = x.Body,
                ReleaseYear = x.ReleaseYear,
                Color = x.Color,
                Model = x.Model
            }).ToList();
            return answer;
        }

        public void Create(VehicleModel vm, HttpContextBase context)
        {
            var database = new CnDbContext();
            var userId = new AuthHelper(context).AuthenticationManager.User.Identity.GetUserId();
            var vehicle = new Vehicle();
            vehicle.UserId = userId;
            vehicle.Body = vm.Body;
            vehicle.Brand = vm.Brand;
            vehicle.Color = vm.Color;
            vehicle.Model = vm.Model;
            vehicle.ReleaseYear = vm.ReleaseYear;
            database.Vehicles.Add(vehicle);
            database.SaveChanges();
        }

        public void Delete(int id, HttpContextBase hc)
        {
            var db = new CnDbContext();
            var checkUser = new CommonHelper().GetAccessToVehicle(hc.User.Identity.GetUserId(), id, db);
            if (!checkUser) { return; }
            db.Vehicles.Remove(db.Vehicles.FirstOrDefault(x => x.Id == id));
            db.SaveChanges();
        }
         
        public VehicleModel GetDataEdit(int Id)
        {
            var db = new CnDbContext();
            var editVehicle = db.Vehicles.FirstOrDefault(x => x.Id == Id);
            var editVehicleModel = new VehicleModel();
            editVehicleModel.Brand = editVehicle.Brand;
            editVehicleModel.Model = editVehicle.Model;
            editVehicleModel.ReleaseYear = editVehicle.ReleaseYear;
            editVehicleModel.Color = editVehicle.Color;
            editVehicleModel.Body = editVehicle.Body;
            editVehicleModel.Id = editVehicle.Id;
            return editVehicleModel;
        }

        public void ChangeData(VehicleModel vm, HttpContextBase hc)
        {
            var data = new CnDbContext();
            var checkUser = new CommonHelper().GetAccessToVehicle(hc.User.Identity.GetUserId(), vm.Id, data);
            if (!checkUser) { return; }
            var vehicleEdit = data.Vehicles.FirstOrDefault(x => x.Id == vm.Id);
            vehicleEdit.Brand = vm.Brand;
            vehicleEdit.Model = vm.Model;
            vehicleEdit.Body = vm.Body;
            vehicleEdit.Color = vm.Color;
            vehicleEdit.ReleaseYear = vm.ReleaseYear;
            vehicleEdit.Id = vm.Id;
            data.SaveChanges();
        }
    }
}