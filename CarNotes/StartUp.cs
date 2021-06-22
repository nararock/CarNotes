using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarNotes.Startup))]
namespace CarNotes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);       
        }
    }
}