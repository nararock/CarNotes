using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CarNotes.Classes
{
    public class RegistrationHelper
    {
        public async Task<IEnumerable<string>> Register(RegistrationModel rm, HttpContextBase context)
        {
            var authHelper = new AuthHelper(context);
            var user = new CnDb.User
            {
                Name = rm.Name,
                Email = rm.Email,
                UserName = rm.Email,
                LastVisit = DateTime.Now
            };
            var result = await authHelper.UserManager.CreateAsync(user, rm.Password);
            return result.Errors;
        }
    }
}