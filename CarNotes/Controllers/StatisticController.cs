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
    public class StatisticController : Controller
    {
        // GET: Statistic
        public ActionResult Index(int? vehicleId)
        {
            if (vehicleId != null)
            {
                ViewBag.VehicleId = vehicleId;
                ViewBag.Name = "Статистика";
                return View();
            }
            return Redirect("~/Home/Resolve?ReturnURL=/Statistic/Index");
        }

        /// <summary>
        /// получение данных для построения круговой диаграммы с затратами за определенный период времени
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public ActionResult GetAllExpenseData(int vehicleId)
        {
            var dataAllExpense = new StatisticHelper().GetDataForPieAllExpense(vehicleId);
            var result = new JsonResult();
            result.Data = dataAllExpense;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
    }
}