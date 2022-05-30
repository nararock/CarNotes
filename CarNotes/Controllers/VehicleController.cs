using CarNotes.Classes;
using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarNotes.Controllers
{
    [Authorize]
    public class VehicleController : Controller
    {
        // GET: Vehicle
        public ActionResult Index(int? vehicleId)
        {
            ViewBag.VehicleId = vehicleId;
            ViewBag.Name = "Гараж";
            ViewBag.IsChecked = true;
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
            new VehicleHelper().Delete(id, HttpContext);
            return Redirect("/Vehicle/Index");
        }

        public ActionResult Get(int Id)
        {
            var VehicleEdit = new VehicleHelper().GetDataEdit(Id);
            var result = new JsonResult();
            result.Data = VehicleEdit;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
         [HttpPost]
         public ActionResult Edit(VehicleModel vm)
        {
            new VehicleHelper().ChangeData(vm, HttpContext);
            return Redirect("/Vehicle/Index");
        }
    }
}