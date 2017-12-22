using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Meghan_BugTracker.Startup))]
namespace Meghan_BugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
