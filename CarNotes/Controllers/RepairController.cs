using CarNotes.Classes;
using CarNotes.Models;
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

        [HttpPost]
        public ActionResult RepairEdit(RepairModel rm)
        {
            new RepairHelper().ChangeData(rm);
            return Redirect("/Home/GoToRepairEvents");
        }

        public ActionResult GetSystem()
        {
            var resultList = new RepairHelper().GetSystemList();
            var resultJson = new JsonResult();
            resultJson.Data = resultList;
            resultJson.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultJson;
        }
    }
}