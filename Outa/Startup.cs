using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Outa.Startup))]
namespace Outa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
