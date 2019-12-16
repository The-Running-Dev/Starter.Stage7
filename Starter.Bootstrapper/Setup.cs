﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Starter.Broker.Azure;
using Starter.Data.Entities;
using Starter.Data.Services;
using Starter.Data.Consumers;
using Starter.Data.ViewModels;
using Starter.Data.Repositories;
using Starter.Framework.Loggers;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupType"></param>
        public static ServiceProvider BootstrapConfig(IServiceCollection services, SetupType setupType = SetupType.Debug)
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

            // Create a new instance of the configuraiton service using the setup type
            var configService = new ConfigurationService(setupType.GetDescription());

            services.AddSingleton<IConfigurationService>(x => configService);
            services.AddSingleton<ISettings>(x => configService.Get<Settings>("Settings"));

            return services.BuildServiceProvider();
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

            RegisterService(services);
        }

        /// <summary>
        /// Creates a new host builder for running from the console
        /// </summary>
        /// <returns></returns>
        public static IHostBuilder BootstrapHost()
        {
            var configServices = new ServiceCollection();

            var provider = BootstrapConfig(configServices);
            var settings = provider.GetService<ISettings>();
            var configurationService = provider.GetService<IConfigurationService>();
            
            return new HostBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.AddConfiguration(configurationService.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddApplicationInsights(settings.ApplicationInsightsInstrumentationKey);
                })
                .ConfigureServices((services) =>
                {
                    // Provide the existing instances of the settings and the configuration service
                    services.AddSingleton<ISettings>((x) => settings);
                    services.AddSingleton<IConfigurationService>((x) => configurationService);

                    // Register the rest of the services
                    RegisterService(services);
                });
        }

        /// <summary>
        /// Registers application services,
        /// and creates a new instance of the IOC wrapper
        /// </summary>
        /// <param name="services"></param>
        private static void RegisterService(IServiceCollection services)
        {
            services.AddTransient<IApiClient, ApiClient>();
            services.AddTransient<ILogger, ApplicationInsightsLogger>();

            services.AddTransient<ICatRepository, CatRepository>();
            services.AddTransient<IMessageBroker<Cat>, AzureMessageBroker<Cat>>();
            services.AddTransient<IMessageConsumer<Cat>, MessageConsumer<Cat>>();
            services.AddTransient<IMessageConsumerService, MessageConsumerService>();
            
            services.AddTransient<ICatService, CatService>();
            services.AddTransient<IMainViewModel, MainViewModel>();

            services.AddHostedService<MessageConsumerService>();

            IocWrapper.Instance = new IocWrapper(services.BuildServiceProvider());
        }
    }
}