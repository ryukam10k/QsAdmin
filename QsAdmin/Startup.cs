using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QsAdmin.Startup))]
namespace QsAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
