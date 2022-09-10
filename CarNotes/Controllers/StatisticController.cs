using CarNotes.Classes;
using CarNotes.CnDb;
using CarNotes.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
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
        public ActionResult GetDataForCommonStatistic(int vehicleId)
        {
            var dataCommonStatistic = new StatisticHelper().GetDataForCommonStatistic(vehicleId);
            var result = new JsonResult();
            result.Data = dataCommonStatistic;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        /// <summary>
        /// получение данных для построения графика (bar chart) расхода топлива 
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public ActionResult GetDataForFuelFlowStatistic(int vehicleId, string dateFrom, string dateTo)
        {
            DateTime dateFromParam = DateTime.Now.AddDays(-90);
            if (!string.IsNullOrWhiteSpace(dateFrom) && dateFrom != "undefined")
            {
                dateFromParam = DateTime.ParseExact(dateFrom, "dd.MM.yyyy", null);
            }

            DateTime dateToParam = DateTime.Now;
            if (!string.IsNullOrEmpty(dateTo) && dateTo != "undefined")
            {
                dateToParam = DateTime.ParseExact(dateTo, "dd.MM.yyyy", null);
            }
            var dataFuelFlowStatistic = new StatisticHelper().GetDataForFuelFlowStatistic(vehicleId, dateFromParam, dateToParam);
            var result = new JsonResult();
            result.Data = dataFuelFlowStatistic;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public ActionResult GetDataForCommonStatisticTable(int vehicleId)
        {
            var dataCommonStatistic = new StatisticHelper().GetDataForCommonInformation(vehicleId);
            var result = new JsonResult();
            result.Data = dataCommonStatistic;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
    }
}