using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HotSpot.Startup))]
namespace HotSpot
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
