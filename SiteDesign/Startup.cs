using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SiteDesign.Startup))]
namespace SiteDesign
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
