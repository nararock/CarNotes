using CarNotes.CnDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CarNotes.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.IsAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            return View();
        }
    }
}
