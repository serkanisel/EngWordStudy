using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EWS.Web.Startup))]
namespace EWS.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
