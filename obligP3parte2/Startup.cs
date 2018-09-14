using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(obligP3parte2.Startup))]
namespace obligP3parte2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
