using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Neurocom.Startup))]
namespace Neurocom
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
