using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Classes
{
    public class RegistrationHelper
    {
        public void Register(RegistrationModel rm, HttpContextBase context)
        {
            var authHelper = new AuthHelper(context);
            var user = new CnDb.User
            {
                Name = rm.Name,
                Email = rm.Email,
                UserName = rm.Email
            };
            authHelper.UserManager.CreateAsync(user);
            authHelper.UserManager.AddPasswordAsync(user.Id, rm.Password);
        }
    }
}