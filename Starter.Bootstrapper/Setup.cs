using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using RestSharp;

using Starter.Broker.Azure;
using Starter.Data.Entities;
using Starter.Data.Services;
using Starter.Data.Consumers;
using Starter.Data.ViewModels;
using Starter.Data.Repositories;
using Starter.Framework.Clients;
using Starter.Framework.Extensions;
using Starter.Configuration.Entities;
using Starter.Configuration.Services;
using Starter.Repository.Repositories;

namespace Starter.Bootstrapper
{
    /// <summary>
    /// Sets up the dependency resolution for the project
    /// </summary>
    public static class Setup
    {
        public static ISettings Settings { get; private set; }

        public static IApiSettings ApiSettings { get; private set; }

        public static IConfigurationService ConfigurationService { get; private set; }
        
        /// <summary>
        /// Sets up the configuration for the application
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupType"></param>
        public static void BootstrapConfig(IServiceCollection services, SetupType setupType = SetupType.Debug)
        {
            // Switch the setup type based on the compile time constant
#if RELEASE
            if (setupType != SetupType.Test)
            {
                setupType = SetupType.Release;
            }
#endif

            if (services == null)
            {
                services = new ServiceCollection();
            }

            // Create a new instance of the configuration service using the setup type
            var configService = new ConfigurationService(setupType.GetDescription());

            services.AddSingleton<IConfigurationService>(x => configService);
            services.AddSingleton<ISettings>(x => configService.Get<Settings>("Settings"));
            services.AddSingleton<IApiSettings>(x => configService.Get<ApiSettings>("ApiSettings"));

            var provider = services.BuildServiceProvider();

            Settings = provider.GetService<ISettings>();
            ApiSettings = provider.GetService<IApiSettings>();
            ConfigurationService = provider.GetService<IConfigurationService>();
        }

        /// <summary>
        /// Registers the services with the DI container,
        /// by using a new service collection
        /// </summary>
        /// <param name="setupType"></param>
        public static void Bootstrap(SetupType setupType = SetupType.Debug)
        {
            Bootstrap(new ServiceCollection(), setupType);
        }

        /// <summary>
        /// Registers services with the DI container,
        /// based on the setup type
        /// </summary>
        public static void Bootstrap(IServiceCollection services, SetupType setupType = SetupType.Debug)
        {
            BootstrapConfig(services, setupType);

            RegisterServices(services);
        }

        /// <summary>
        /// Sets up the MessageService to run from the console
        /// </summary>
        /// <returns></returns>
        public static IHostBuilder BootstrapConsumer()
        {
            var configServices = new ServiceCollection();

            BootstrapConfig(configServices);

            return new HostBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.AddConfiguration(ConfigurationService.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddApplicationInsights(Settings.ApplicationInsightsInstrumentationKey);
                })
                .ConfigureServices((services) =>
                {
                    // Provide the existing instances of the settings and the configuration service
                    services.AddSingleton<ISettings>((x) => Settings);
                    services.AddSingleton<IApiSettings>((x) => ApiSettings);
                    services.AddSingleton<IConfigurationService>((x) => ConfigurationService);

                    // Register the rest of the services
                    RegisterServices(services);
                });
        }

        /// <summary>
        /// Registers application services,
        /// and creates a new instance of the IOC wrapper
        /// </summary>
        /// <param name="services"></param>
        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IRestClient, RestClient>();
            services.AddTransient<IApiClient, ApiClient>();

            services.AddTransient<ILoggerFactory, LoggerFactory>();

            // Message broker, consumer and service
            services.AddTransient<IMessageBroker<Cat>, AzureMessageBroker<Cat>>();
            services.AddTransient<IMessageConsumer<Cat>, MessageConsumer<Cat>>();
            services.AddTransient<IMessageService<Cat>, MessageService<Cat>>();

            services.AddTransient<ICatRepository, CatRepository>();
            services.AddTransient<ICatService, CatService>();

            services.AddTransient<IMainViewModel, MainViewModel>();

            services.AddTransient<IQueueClient>(x => new QueueClient(Settings.ServiceBusConnection, Settings.ServiceBusQueue));

            services.AddHostedService<MessageService<Cat>>();

            IocWrapper.Instance = new IocWrapper(services.BuildServiceProvider());
        }
    }
}