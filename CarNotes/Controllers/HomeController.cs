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
                ViewBag.IsChecked = false;
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var userIdCheck = new AuthHelper(HttpContext).AuthenticationManager.User.Identity.GetUserId();
                    if (new CnDbContext().Users.Find(userIdCheck).Vehicles.Any(v => v.Id == vehicleId))
                    {
                        ViewBag.IsChecked = true;
                    }
                }
                var common = new CommonHelper();
                var cm = common.CreateList((int)vehicleId);
                if (cm == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
                ViewBag.Name = "Общая таблица";
                return View(cm);
            }
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("~/Login/Index");
            }
            var vehicleIdCookie = HttpContext.Request.Cookies.Get("vehicleId")?.Value;
            int vehicleIdNumber;
            if (int.TryParse(vehicleIdCookie, out vehicleIdNumber))
            {
                return Redirect("~/Home/Index?vehicleId=" + vehicleIdNumber);
            }
            var userId = new AuthHelper(HttpContext).AuthenticationManager.User.Identity.GetUserId();
            vehicleId = new CnDbContext().Users.Find(userId).Vehicles.FirstOrDefault()?.Id;
            if (vehicleId != null) return Redirect("~/Home/Index?vehicleId=" + vehicleId);
            return Redirect("~/Vehicle/Index");
        }
           
        public ActionResult DeleteEvent(string record, int id)
        {
            if(record == "Refuel")
            {
                new RefuelHelper().Delete(id, HttpContext);
            }
            else if(record == "Repair")
            {
                new RepairHelper().Delete(id, HttpContext);
            }
            return Redirect("~/Home/Index");
        }
    }
}