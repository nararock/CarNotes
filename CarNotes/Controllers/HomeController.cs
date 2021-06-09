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
            CnDbContext db = new CnDbContext();
            return View(db.RefuelEvents);
        }

        [HttpGet]
        public ActionResult CreateNewEvent()
        {
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
            return View();
        }

        public ActionResult CreateNewRepairEvent()
        {
            return View();
        }
    }
}