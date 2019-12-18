using NUnit.Framework;

using Starter.Mocks;
using Starter.Bootstrapper;
using Starter.Data.Entities;
using Starter.Data.Services;
using Starter.Configuration.Entities;

namespace Starter.Broker.Azure.Tests
{
    /// <summary>
    /// Implements tests setup
    /// </summary>
    public class TestsBase
    {
        protected QueueClientMock QueueClientMock { get; private set; }

        protected IMessageBroker<Cat> MessageBroker { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Setup.Bootstrap(SetupType.Test);

            var settings = IocWrapper.Instance.GetService<ISettings>();

            QueueClientMock = new QueueClientMock();

            MessageBroker = new AzureMessageBroker<Cat>(settings, QueueClientMock.Instance);
        }
    }
}