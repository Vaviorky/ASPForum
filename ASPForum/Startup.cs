using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASPForum.Startup))]
namespace ASPForum
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
