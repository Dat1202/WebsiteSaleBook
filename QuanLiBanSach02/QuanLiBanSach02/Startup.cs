using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuanLiBanSach02.Startup))]
namespace QuanLiBanSach02
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
