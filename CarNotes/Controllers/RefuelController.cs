using CarNotes.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarNotes.Controllers
{
    public class RefuelController : Controller
    {
        // GET: Refuel
        public ActionResult Delete(int id)
        {
            new RefuelHelper().Delete(id, HttpContext);
            return Redirect("~/Home/GoToRefuelEvents");
        }
    }
}