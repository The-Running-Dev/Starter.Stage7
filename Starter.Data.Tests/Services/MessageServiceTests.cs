using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

namespace Starter.Data.Tests.Services
{
    /// <summary>
    /// Tests for the MessageService class
    /// </summary>
    [TestFixture]
    public class MessageServiceTests: TestsBase
    {
        [Test]
        public void MessageConsumerService_NewInstance_Successful()
        {
            MessageService.Should().NotBeNull();
        }

        [Test]
        public void OnDataReceived_Successful()
        {
            //_logger.LogInformation($"{message.Command}, {message.Type}, {message.Entity.ToJson()}");
            //_consumer.Consume(message);
        }

        [Test]
        public async Task StartAsync_Successful()
        {
            await MessageService.StartAsync(new CancellationToken());
            //_logger.LogInformation($"Starting {nameof(MessageConsumerService)}...");
            //await Task.Run(() => { _broker.Receive(); });

            //MessageBrokerMock.Verify();
        }

        [Test]
        public async Task StopAsync_Successful()
        {
            await MessageService.StopAsync(new CancellationToken());
            //_logger.LogInformation($"{nameof(MessageConsumerService)} Stopped...");
            //return Task.CompletedTask;
        }
    }
}