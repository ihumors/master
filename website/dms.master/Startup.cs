using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dms.Master.Startup))]
namespace Dms.Master
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
