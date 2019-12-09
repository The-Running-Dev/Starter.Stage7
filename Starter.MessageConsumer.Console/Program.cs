using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using Starter.Bootstrapper;
using Starter.Framework.Entities;

namespace Starter.MessageConsumer.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            Setup.Bootstrap(services);

            var startup = new Startup();
            var settings = IocWrapper.Instance.GetService<ISettings>();

            var builder = new HostBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.AddConfiguration(startup.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddApplicationInsights(settings.ApplicationInsightsInstrumentationKey);
                })
                .ConfigureServices(s =>
                {
                    Setup.Bootstrap(s);
                });

            await builder.RunConsoleAsync();
        }
    }
}
