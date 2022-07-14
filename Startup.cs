using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pruebas2.Startup))]
namespace Pruebas2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
