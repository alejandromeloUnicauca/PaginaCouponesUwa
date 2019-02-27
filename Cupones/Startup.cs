using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cupones.Startup))]
namespace Cupones
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
