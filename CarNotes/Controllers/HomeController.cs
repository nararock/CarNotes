using CarNotes.CnDb;
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

        public ActionResult CreateNewEvent()
        {
            return View();
        }
    }
}