using CarNotes.Classes;
using CarNotes.CnDb;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CarNotes
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_EndRequest()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var db = new CnDbContext();                
                var userId = Context.User.Identity.GetUserId();
                var userDb = db.Users.FirstOrDefault(x => x.Id == userId);
                if (userDb != null)
                {
                    userDb.LastVisit = DateTime.Now;
                    db.SaveChanges();
                }
            }
        }
    }
}
