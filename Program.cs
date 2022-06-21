using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace CALENDAR_Version_3._0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        /*private static void ConfigureServicesDelegate(HostBuilderContext hostingContext, IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            JobManager.Initialize(new JobRegistry(serviceProvider));
        }*/
    }
}
