using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(testIdentity.Startup))]
namespace testIdentity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
