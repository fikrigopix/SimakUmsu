using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SIA_Universitas.Startup))]
namespace SIA_Universitas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}