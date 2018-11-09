using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AnimalDB.Startup))]
namespace AnimalDB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
