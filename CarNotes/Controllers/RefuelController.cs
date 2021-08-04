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
        public ActionResult Edit(int id)
        {
            var refuelEdit = new RefuelHelper().GetDataEdit(id);
            return View(refuelEdit);
        }

        [HttpPost]
        public void Edit(RefuelModel rm)
        {
            new RefuelHelper().ChangeData(rm);
        }
    }
}