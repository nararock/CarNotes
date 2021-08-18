using CarNotes.Classes;
using CarNotes.Models;
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

        [HttpGet]
        public ActionResult RefuelEdit(int id)
        {
            var refuelEdit = new RefuelHelper().GetDataEdit(id);
            var result = new JsonResult();
            result.Data = refuelEdit;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [HttpPost]
        public ActionResult RefuelEdit(RefuelModel rm)
        {
            new RefuelHelper().ChangeData(rm);
            return Redirect("/Home/GoToRefuelEvents");
        }
    }
}