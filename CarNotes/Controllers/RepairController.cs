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
    public class RepairController : Controller
    {
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
                        HttpContext.Response.Cookies.Set(new HttpCookie("vehicleId", vehicleId.ToString()));
                    }
                }
                var common = new RepairHelper();
                var cm = common.GetList((int)vehicleId);
                if (cm == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
                ViewBag.Name = "Ремонт";
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
                return Redirect("~/Repair/Index?vehicleId=" + vehicleIdNumber);
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

        [HttpGet]
        public ActionResult Get(int id)
        {
            var repairEdit = new RepairHelper().GetDataEdit(id);
            var result = new JsonResult();
            result.Data = repairEdit;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [HttpPost]
        public ActionResult Edit(RepairModel rm)
        {
            var vehicleId = int.Parse(HttpContext.Request.Cookies.Get("vehicleId").Value);
            new RepairHelper().ChangeData(rm, vehicleId);
            return Redirect("/Repair/Index");
        }

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