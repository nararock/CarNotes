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
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!await new LoginHelper().Login(loginModel, HttpContext))
            {
                ModelState.AddModelError("", "Не верно введен логин или пароль");
                return View();
            }
            return Redirect("/Common/Index/");
        }

        public ActionResult LogOut()
        {
            new LoginHelper().LogOut(HttpContext);
            return Redirect("~");
        }
    }
}