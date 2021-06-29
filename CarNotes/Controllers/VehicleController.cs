using CarNotes.Classes;
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
    }
}