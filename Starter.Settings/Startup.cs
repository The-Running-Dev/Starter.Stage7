using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Starter.Configuration
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            //var configBuilder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", true, true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
            //    .AddEnvironmentVariables();

            //Configuration = configBuilder.Build();

            //new HostBuilder()
            //    .ConfigureLogging(logging =>
            //    {
            //        logging.AddConfiguration(Configuration.GetSection("Logging"));
            //        logging.AddConsole();
            //        logging.AddDebug();
            //        logging.AddApplicationInsights(settings.ApplicationInsightsInstrumentationKey);
            //    })
            //    .ConfigureServices(s =>
            //    {
            //        Setup.Bootstrap(s);
            //    });
        }
    }
}