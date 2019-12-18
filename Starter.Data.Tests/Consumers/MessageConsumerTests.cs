using System;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Starter.Bootstrapper;
using Starter.Data.Consumers;
using Starter.Data.Entities;
using Starter.Mocks;

namespace Starter.Data.Tests.Consumers
{
    /// <summary>
    /// Tests for the MessageConsumer class
    /// </summary>
    [TestFixture]
    public class MessageConsumerTests: TestsBase
    {
        [Test]
        public void New_MessageConsumerInstance_Successful()
        {
            MessageConsumer.Should().NotBeNull();
        }

        [Test]
        public void Consume_CreateMessage_Successful()
        {
            Consume_Message(MessageCommand.Create);
            
            ApiClientMock.VerifyCreate();
        }

        [Test]
        public void Consume_UpdateMessage_Successful()
        {
            Consume_Message(MessageCommand.Update);
            
            ApiClientMock.VerifyUpdate();
        }

        [Test]
        public void Consume_DeleteMessage_Successful()
        {
            Consume_Message(MessageCommand.Delete);

            ApiClientMock.VerifyDelete();
        }

        [Test]
        public void Consume_UnknownMessage_Fails()
        {
            var logger = IocWrapper.Instance.GetService<ILogger>();
            var consumer = new MessageConsumer<Cat>(ApiClientMock.Instance, logger);

            var cat = TestData.Cats.FirstOrDefault();
            var message = new Message<Cat>(MessageCommand.Unknown, cat);

            Assert.Throws<ArgumentOutOfRangeException>(() => consumer.Consume(message));
        }

        private void Consume_Message(MessageCommand command)
        {
            var cat = TestData.Cats.FirstOrDefault();
            var message = new Message<Cat>(command, cat);

            MessageConsumer.Consume(message);
        }
    }
}