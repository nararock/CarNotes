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
    public class ExpenseController : Controller
    {
        [AllowAnonymous]
        // GET: Expense
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
                var expenseModel = new ExpenseHelper().GetList((int)vehicleId, pageNumber, pageSize);                
                if (expenseModel == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
                var database = new CnDbContext();
                var countEvent = database.Expenses.Where(e => e.VehicleId == vehicleId).Count();
                var pageModel = new PageExpenseTable(countEvent, pageNumber, pageSize);
                pageModel.PageList = expenseModel;
                ViewBag.Name = "Расходы";
                return View(pageModel);
            }
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("~/Login/Index");
            }
            var vehicleIdCookie = HttpContext.Request.Cookies.Get("vehicleId")?.Value;
            int vehicleIdNumber;
            if (int.TryParse(vehicleIdCookie, out vehicleIdNumber))
            {
                if (!new CnDbContext().Vehicles.Any(v => v.Id == vehicleIdNumber))
                {
                    HttpContext.Response.Cookies.Remove("vehicleId");
                }
                else return Redirect("~/Expense/Index?vehicleId=" + vehicleIdNumber);
            }
            var userId = new AuthHelper(HttpContext).AuthenticationManager.User.Identity.GetUserId();
            vehicleId = new CnDbContext().Users.Find(userId).Vehicles.FirstOrDefault()?.Id;
            if (vehicleId != null) return Redirect("~/Expense/Index?vehicleId=" + vehicleId);
            return Redirect("~/Vehicle/Index");
        }

        [HttpPost]
        public ActionResult Edit(ExpenseEditModel em)
        {
            var vehicleId = int.Parse(HttpContext.Request.Cookies.Get("vehicleId").Value);
            new ExpenseHelper().ChangeData(em, vehicleId, HttpContext);
            return Redirect("/Expense/Index");
        }
        [AllowAnonymous]
        public ActionResult Get(int id)
        {
            var expenseEdit = new ExpenseHelper().GetExpenseEditList(id);
            var result = new JsonResult();
            result.Data = expenseEdit;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public ActionResult Delete(int id)
        {
            new ExpenseHelper().Delete(id, HttpContext);
            return Redirect("~/Expense/Index");
        }
    }
}