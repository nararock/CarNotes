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
    public class CommonController : Controller
    {
        // GET:Common
        public ActionResult Index(int? vehicleId, int pageNumber=1)
        {
            if (vehicleId == null)
            {
                return Redirect("~/Home/Resolve?ReturnURL=/Common/Index");
            }

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
            int pageSize = 1;//количество выводимых событий на 1-ой странице 
            var commonModelList = new CommonHelper().GetList((int)vehicleId, pageNumber, pageSize);
            if (commonModelList == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            var database = new CnDbContext();
            var countEvent = database.RefuelEvents.Where(e => e.VehicleId == vehicleId).Count()
                + database.RepairEvents.Where(e => e.VehicleId == vehicleId).Count() + database.Expenses.Where(e => e.VehicleId == vehicleId).Count();
            var pageModel = new PageCommonTable(countEvent, pageNumber, pageSize);
            pageModel.PageList = commonModelList;
            ViewBag.Name = "Общая таблица";
            return View(pageModel);
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
            return Redirect("~/Common/Index");
        }

        public ActionResult GetLastMileage(int vehicleId)
        {
            var lastMileage =  new CommonHelper().GetLastMileage(vehicleId);
            var result = new JsonResult();
            result.Data = lastMileage;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
    }
}