using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TwitterWebFrontend.Startup))]
namespace TwitterWebFrontend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
