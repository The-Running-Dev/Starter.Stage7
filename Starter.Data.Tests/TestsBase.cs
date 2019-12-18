using NUnit.Framework;
using Microsoft.Extensions.Logging;

using Starter.Mocks;
using Starter.Bootstrapper;
using Starter.Data.Entities;
using Starter.Data.Services;
using Starter.Data.Consumers;
using Starter.Data.ViewModels;

namespace Starter.Data.Tests
{
    /// <summary>
    /// Implements tests setup
    /// </summary>
    public class TestsBase
    {
        protected ApiClientMock ApiClientMock { get; set; }

        protected ICatService CatService { get; private set; }

        protected CatServiceMock CatServiceMock { get; private set; }

        protected QueueClientMock QueueClientMock { get; private set; }

        protected IMessageConsumer<Cat> MessageConsumer { get; private set; }

        protected MessageConsumerMock<Cat> MessageConsumerMock { get; private set; }

        protected MessageService<Cat> MessageService { get; private set; }

        protected MessageConsumerServiceMock<Cat> MessageConsumerServiceMock { get; private set; }

        protected IMessageBroker<Cat> MessageBroker { get; private set; }

        protected MessageBrokerMock<Cat> MessageBrokerMock { get; private set; }

        protected MainViewModel ViewModel { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Setup.Bootstrap(SetupType.Test);

            ApiClientMock = new ApiClientMock();
            CatServiceMock = new CatServiceMock();
            MessageBrokerMock = new MessageBrokerMock<Cat>();
            MessageConsumerMock = new MessageConsumerMock<Cat>();
            MessageConsumerServiceMock = new MessageConsumerServiceMock<Cat>();
            QueueClientMock = new QueueClientMock();

            var logger = IocWrapper.Instance.GetService<ILogger>();

            CatService = new CatService(MessageBrokerMock.Instance, ApiClientMock.Instance);
            MessageConsumer = new MessageConsumer<Cat>(ApiClientMock.Instance, logger);
            MessageService = new MessageService<Cat>(MessageBrokerMock.Instance, MessageConsumerMock.Instance, logger);
            ViewModel = new MainViewModel(CatServiceMock.Instance);
        }
    }
}