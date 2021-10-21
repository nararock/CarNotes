using CarNotes.Classes;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarNotes.Controllers
{
    public class VehicleController : Controller
    {
        // GET: Vehicle
        public ActionResult Index()
        {
            var vehicles = new VehicleHelper().GetVehicles(HttpContext);
            return View(vehicles);
        }

        [HttpPost]
        public ActionResult Add(VehicleModel vm)
        {
            new VehicleHelper().Create(vm, HttpContext);
            return Redirect("Index");
        }

        public ActionResult Delete(int id)
        {
            new VehicleHelper().Delete(id);
            return Redirect("/Vehicle/Index");
        }
    }
}