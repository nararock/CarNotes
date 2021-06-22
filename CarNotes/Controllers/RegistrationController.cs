using CarNotes.Classes;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarNotes.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegistrationModel rm)
        {
            new RegistrationHelper().Register(rm, HttpContext);
            return View("EndOfRegistration");
        }
    }
}