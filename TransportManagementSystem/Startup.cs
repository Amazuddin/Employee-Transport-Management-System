using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TransportManagementSystem.Startup))]
namespace TransportManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
