using Microsoft.Azure.Functions.Extensions.DependencyInjection;

using Starter.Bootstrapper;
using Starter.MessageBroker.Consumer.Azure;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Starter.MessageBroker.Consumer.Azure
{
    /// <summary>
    /// Sets up the dependency injection for the Azure function
    /// </summary>
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            Setup.Bootstrap(builder.Services);
        }
    }
}