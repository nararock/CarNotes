using CarNotes.Classes;
using CarNotes.CnDb;
using Microsoft.AspNet.Identity;
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
        /// <summary>
        /// Функция вызывается в методах Index контроллеров при условии vehicleId==null, проверяет зарегистрирован ли
        /// пользователь, есть ли у него автомобили и в зависимости от результата функция перенаправляет на нужную страницу.
        /// </summary>
        /// <param name="ReturnURL"></param>
        /// <returns></returns>
        public ActionResult Resolve(string ReturnURL)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("~/Login/Index");
            }
            // Проверяем, что в куке было число
            var vehicleIdCookie = HttpContext.Request.Cookies.Get("vehicleId")?.Value;
            int vehicleIdNumber;
            if (int.TryParse(vehicleIdCookie, out vehicleIdNumber))
            {
                if (!new CnDbContext().Vehicles.Any(v => v.Id == vehicleIdNumber))
                {
                    HttpContext.Response.Cookies.Remove("vehicleId");
                }
                else return Redirect(ReturnURL + "?vehicleId=" + vehicleIdNumber);
            }
            var userId = new AuthHelper(HttpContext).AuthenticationManager.User.Identity.GetUserId();
            var vehicleId = new CnDbContext().Users.Find(userId).Vehicles.FirstOrDefault()?.Id;
            if (vehicleId != null) return Redirect(ReturnURL + "?vehicleId=" + vehicleId);
            return Redirect("~/Vehicle/Index");
        }
    }
}
