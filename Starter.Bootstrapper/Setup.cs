using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using Starter.Data.Services;
using Starter.Data.Consumers;
using Starter.Data.Entities;
using Starter.Data.ViewModels;
using Starter.Data.Repositories;

using Starter.Framework.Clients;
using Starter.Framework.Entities;
using Starter.Framework.Loggers;
using Starter.MessageBroker.Azure;
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
        /// <param name="setupType"></param>
        public static void Bootstrap(SetupType setupType = SetupType.Debug)
        {
            Bootstrap(new ServiceCollection(), setupType);
        }

        /// <summary>
        /// Registers service implementations with the DI container,
        /// based on the setup type
        /// </summary>
        public static void Bootstrap(IServiceCollection services, SetupType setupType = SetupType.Debug)
        {
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

            switch (setupType)
            {
                case SetupType.Release:
                    services.AddSingleton<ISettings, Settings>();

                    break;
                case SetupType.Debug:
                    services.AddSingleton<ISettings, SettingsDev>();

                    break;
                case SetupType.Test:
                    services.AddSingleton<ISettings, SettingsTest>();

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(setupType), setupType, null);
            }

            services.AddTransient<IApiClient, ApiClient>();
            services.AddTransient<ILogger, ApplicationInsightsLogger>();

            services.AddTransient<ICatRepository, CatRepository>();
            services.AddTransient<IMessageBroker<Cat>, AzureMessageBroker<Cat>>();
            services.AddTransient<IMessageConsumer<Cat>, MessageConsumer<Cat>>();
            services.AddTransient<IMessageConsumerService, MessageConsumerService>();
            services.AddTransient<ICatService, CatService>();
            services.AddTransient<IMainViewModel, MainViewModel>();
            services.AddHostedService<MessageConsumerService>();

            var serviceProvider = services.BuildServiceProvider();

            IocWrapper.Instance = new IocWrapper(serviceProvider);
        }
    }
}