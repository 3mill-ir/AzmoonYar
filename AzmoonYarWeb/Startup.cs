using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzmoonYarWeb.Startup))]
namespace AzmoonYarWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
