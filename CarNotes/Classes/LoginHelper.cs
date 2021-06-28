using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CarNotes.Classes
{
    public class LoginHelper
    {
        public async Task<bool> Login(LoginModel lm, HttpContextBase context) 
        {
            var authHelper = new AuthHelper(context);
            var result = await authHelper.SignInManager.PasswordSignInAsync(lm.Login, lm.Password, true, true);
            return result == Microsoft.AspNet.Identity.Owin.SignInStatus.Success;
        }

        public void LogOut(HttpContextBase context)
        {
            new AuthHelper(context).AuthenticationManager.SignOut();
        }
    }
}