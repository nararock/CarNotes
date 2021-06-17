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
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Name = "Общая таблица";
            var common = new CommonHelper();
            var cm = common.CreateList();
            return View(cm);
        }

        public ActionResult GoToRefuelEvents()
        {
            ViewBag.Name = "Заправка";
            CnDbContext db = new CnDbContext();
            return View(db.RefuelEvents);
        }

        [HttpGet]
        public ActionResult CreateNewEvent()
        {
            ViewBag.Name = "Новая заправка";
            return View();
        }

        [HttpPost]
        public ActionResult CreateNewEvent(RefuelModel rm)
        {
            new RefuelHelper().SaveToDataBase(rm);
            return RedirectToAction("Index");
        }

        public ActionResult GoToRepairEvents()
        {
            CnDbContext db = new CnDbContext();
            return View(db.RepairEvents);
        }

        [HttpGet]
        public ActionResult CreateNewRepairEvent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateNewRepairEvent(RepairModel rm)
        {
            new RepairHelper().SaveToDataBase(rm);
            return RedirectToAction("GoToRepairEvents");
        }
    }
}