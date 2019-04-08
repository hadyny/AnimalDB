using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(AnimalDBCore.Areas.Identity.IdentityHostingStartup))]
namespace AnimalDBCore.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}