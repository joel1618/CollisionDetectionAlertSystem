using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CollisionDetectionAlertSystem.Startup))]
namespace CollisionDetectionAlertSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
