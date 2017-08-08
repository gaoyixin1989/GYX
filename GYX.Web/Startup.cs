using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GYX.Web.Startup))]
namespace GYX.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
