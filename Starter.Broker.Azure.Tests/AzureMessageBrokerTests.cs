using System;

using NUnit.Framework;
using FluentAssertions;

using Starter.Data.Entities;

namespace Starter.Broker.Azure.Tests
{
    /// <summary>
    /// Tests for the AzureMessageBroker class
    /// </summary>
    [TestFixture]
    public class AzureMessageBrokerTests: TestsBase
    {
        [Test]
        public void AzureMessageBroker_NewInstance_Successful()
        {
            MessageBroker.Should().NotBeNull();
        }

        [Test]
        public void Register_Successful()
        {
            MessageBroker.Register();

            QueueClientMock.VerifyRegister();
        }

        [Test]
        public void Send_Successful()
        {
            MessageBroker.Send(new Message<Cat>(MessageCommand.Create, new Cat()));

            QueueClientMock.VerifySend();
        }

        [Test]
        public void Send_WithNullClient_Fails()
        {
            var broker = new AzureMessageBroker<Cat>(null, null);

            Assert.ThrowsAsync<NullReferenceException>(() =>
                broker.Send(new Message<Cat>(MessageCommand.Create, null)));
        }
    }
}