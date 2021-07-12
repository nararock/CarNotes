using CarNotes.Classes;
using CarNotes.CnDb;
using CarNotes.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarNotes.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(int? vehicleId)
        {
            if (vehicleId != null)
            {
                var common = new CommonHelper();
                var cm = common.CreateList((int)vehicleId);
                if (cm == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
                ViewBag.Name = "Общая таблица";
                return View(cm);
            }
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("~/RegistrationController/Index");
            }
            var vehicleIDCookie = HttpContext.Request.Cookies.Get("vehicleId")?.Value;
            if (vehicleIDCookie != null)
            {
                return Redirect("~/Home/Index?vehicleId=" +  vehicleIDCookie);
            }
            var userId = new AuthHelper(HttpContext).AuthenticationManager.User.Identity.GetUserId();
            vehicleId = new CnDbContext().Users.Find(userId).Vehicles.FirstOrDefault().Id;
            if (vehicleId != null) return Redirect("~/Home/Index?vehicleId=" + vehicleId);
            return Redirect("~/Vehicle/Index");
        }

        public ActionResult GoToRefuelEvents(int? vehicleId)
        {
            if (vehicleId != null)
            {
                var common = new RefuelHelper();
                var cm = common.GetList((int)vehicleId);
                if (cm == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
                ViewBag.Name = "Заправка";
                return View(cm);
            }
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("~/RegistrationController/Index");
            }
            var vehicleIDCookie = HttpContext.Request.Cookies.Get("vehicleId")?.Value;
            if (vehicleIDCookie != null)
            {
                return Redirect("~/Home/GoToRefuelEvents?vehicleId=" + vehicleIDCookie);
            }
            var userId = new AuthHelper(HttpContext).AuthenticationManager.User.Identity.GetUserId();
            vehicleId = new CnDbContext().Users.Find(userId).Vehicles.FirstOrDefault().Id;
            if (vehicleId != null) return Redirect("~/Home/GoToRefuelEvents?vehicleId=" + vehicleId);
            return Redirect("~/Vehicle/Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateNewEvent(RefuelModel rm)
        {
            var vehicleId = int.Parse(HttpContext.Request.Cookies.Get("vehicleId").Value);
            var Id = new AuthHelper(HttpContext).AuthenticationManager.User.Identity.GetUserId();
            var vehicle = new CnDbContext().Vehicles.FirstOrDefault(x => x.Id == vehicleId);
            if (vehicle == null || vehicle.UserId != Id)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            new RefuelHelper().SaveToDataBase(rm, vehicleId);
            return RedirectToAction("GoToRefuelEvents");
        }

        public ActionResult GoToRepairEvents()
        {
            ViewBag.Name = "Ремонт";
            CnDbContext db = new CnDbContext();
            return View(db.RepairEvents);
        }


        [Authorize]
        [HttpPost]
        public ActionResult CreateNewRepairEvent(RepairModel rm)
        {
            var vehicleId = int.Parse(HttpContext.Request.Cookies.Get("vehicleId").Value);
            var Id = new AuthHelper(HttpContext).AuthenticationManager.User.Identity.GetUserId();
            var vehicle = new CnDbContext().Vehicles.FirstOrDefault(x => x.Id == vehicleId);
            if (vehicle == null || vehicle.UserId != Id)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            new RepairHelper().SaveToDataBase(rm, vehicleId);
            return RedirectToAction("GoToRepairEvents");
        }
    }
}