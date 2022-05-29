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
    [Authorize]
    public class RefuelController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index(int? vehicleId)
        {
            if (vehicleId != null)
            {
                ViewBag.VehicleId = vehicleId;
                ViewBag.IsChecked = false;
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var userIdCheck = new AuthHelper(HttpContext).AuthenticationManager.User.Identity.GetUserId();
                    if (new CnDbContext().Users.Find(userIdCheck).Vehicles.Any(v => v.Id == vehicleId))
                    {
                        ViewBag.IsChecked = true;
                        HttpContext.Response.Cookies.Set(new HttpCookie("vehicleId", vehicleId.ToString()));
                    }
                }
                var cm = new RefuelHelper().GetList((int)vehicleId);
                if (cm == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
                ViewBag.Name = "Заправки";
                return View(cm);
            }
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("~/Login/Index");
            }
            // Проверяем, что в куке было число
            var vehicleIdCookie = HttpContext.Request.Cookies.Get("vehicleId")?.Value;
            int vehicleIdNumber;
            if (int.TryParse(vehicleIdCookie, out vehicleIdNumber))
            {
                if (!new CnDbContext().Vehicles.Any(v => v.Id == vehicleIdNumber))
                {
                    HttpContext.Response.Cookies.Remove("vehicleId");
                }
                else return Redirect("~/Refuel/Index?vehicleId=" + vehicleIdNumber);
            }
            var userId = new AuthHelper(HttpContext).AuthenticationManager.User.Identity.GetUserId();
            vehicleId = new CnDbContext().Users.Find(userId).Vehicles.FirstOrDefault()?.Id;
            if (vehicleId != null) return Redirect("~/Refuel/Index?vehicleId=" + vehicleId);
            return Redirect("~/Vehicle/Index");
        }

        [HttpPost]
        public ActionResult Create(RefuelModel rm)
        {
            var vehicleId = int.Parse(HttpContext.Request.Cookies.Get("vehicleId").Value);
            var Id = new AuthHelper(HttpContext).AuthenticationManager.User.Identity.GetUserId();
            var vehicle = new CnDbContext().Vehicles.FirstOrDefault(x => x.Id == vehicleId);
            if (vehicle == null || vehicle.UserId != Id)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            new RefuelHelper().SaveToDataBase(rm, vehicleId);
            return RedirectToAction("Index");
        }

        // GET: Refuel
        public ActionResult Delete(int id)
        {
            new RefuelHelper().Delete(id, HttpContext);
            return Redirect("~/Refuel/Index");
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var refuelEdit = new RefuelHelper().GetDataEdit(id);
            var result = new JsonResult();
            result.Data = refuelEdit;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [HttpPost]
        public ActionResult Edit(RefuelModel rm)
        {
            new RefuelHelper().ChangeData(rm, HttpContext);
            return Redirect("/Refuel/Index");
        }

        public ActionResult GetDataForCreateEvent(int vehicleId)
        {
            var dataForCreate = new RefuelHelper().GetDataForCreateEvent(vehicleId);
            var result = new JsonResult();
            result.Data = dataForCreate;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
    }
}