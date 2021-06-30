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
    }
}