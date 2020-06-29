using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Moviet.Areas.Identity.IdentityHostingStartup))]
namespace Moviet.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}