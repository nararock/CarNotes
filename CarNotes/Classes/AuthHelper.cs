using Microsoft.Owin.Security;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using CarNotes.CnDb;

namespace CarNotes.Classes
{
    public class AuthHelper
    {
        public AuthHelper(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        private HttpContextBase _httpContext;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? _httpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? _httpContext.GetOwinContext().Get<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return _httpContext.GetOwinContext().Authentication;
            }
        }

        public string GetName(string userId)
        {
            if (userId == null) return "";

            var db = new CnDbContext();
            string userName = db.Users.Find(userId).Name;
            return userName;
        }
    }

}