using CarNotes.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarNotes.Controllers
{
    public class RepairController : Controller
    {
        // GET: Repair
        public ActionResult Delete(int id)
        {
            new RepairHelper().Delete(id, HttpContext);
            return Redirect("~/Home/GoToRepairEvents");
        }

        [HttpGet]
        public ActionResult RepairEdit(int id)
        {
            var repairEdit = new RepairHelper().GetDataEdit(id);
            var result = new JsonResult();
            result.Data = repairEdit;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
    }
}