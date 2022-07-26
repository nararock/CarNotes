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
    public class RepairController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index(int? vehicleId, int pageNumber = 1)
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
                int pageSize = 10;//количество выводимых событий на 1-ой странице
                var repairModel = new RepairHelper().GetList((int)vehicleId, pageNumber, pageSize);
                if (repairModel == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
                var database = new CnDbContext();
                var countEvent = database.RepairEvents.Where(e => e.VehicleId == vehicleId).Count();
                var pageModel = new PageRepairTable(countEvent, pageNumber, pageSize);
                pageModel.PageList = repairModel;
                ViewBag.Name = "Ремонты";
                return View(pageModel);
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
                else return Redirect("~/Repair/Index?vehicleId=" + vehicleIdNumber);
            }
            var userId = new AuthHelper(HttpContext).AuthenticationManager.User.Identity.GetUserId();
            vehicleId = new CnDbContext().Users.Find(userId).Vehicles.FirstOrDefault()?.Id;
            if (vehicleId != null) return Redirect("~/Repair/Index?vehicleId=" + vehicleId);
            return Redirect("~/Vehicle/Index");
        }

        // GET: Repair
        public ActionResult Delete(int id)
        {
            new RepairHelper().Delete(id, HttpContext);
            return Redirect("~/Repair/Index");
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Get(int id, int? vehicleId)
        {
            var repairEdit = new RepairHelper().GetDataEdit(id, vehicleId);
            var result = new JsonResult();
            result.Data = repairEdit;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [HttpPost]
        public ActionResult Edit(RepairModel rm)
        {
            var vehicleId = int.Parse(HttpContext.Request.Cookies.Get("vehicleId").Value);
            new RepairHelper().ChangeData(rm, vehicleId, HttpContext);
            return Redirect("/Repair/Index");
        }
        [AllowAnonymous]
        public ActionResult GetSystem()
        {
            var resultList = new RepairHelper().GetSystemList();
            var resultJson = new JsonResult();
            resultJson.Data = resultList;
            resultJson.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultJson;
        }
    }
}