using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_04156258.Startup))]
namespace _04156258
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
