using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmartNav.Example.Startup))]
namespace SmartNav.Example
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
