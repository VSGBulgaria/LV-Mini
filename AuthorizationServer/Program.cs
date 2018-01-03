using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AuthorizationServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:55818")
                .UseStartup<Startup>()
                .Build();
    }
}
