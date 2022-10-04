using CarNotes.Classes;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Index(RegistrationModel rm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await new RegistrationHelper().Register(rm, HttpContext);
            if (result.Any())
            {
                foreach(var element in result)
                {
                    ModelState.AddModelError("", element);
                }
                return View();
            }
            ViewBag.Message = "Вы успешно зарегистрировались! Войдите.";
            ViewBag.IsNewRegistration = true;
            return View("~/Views/Login/Index.cshtml");
        }
    }
}