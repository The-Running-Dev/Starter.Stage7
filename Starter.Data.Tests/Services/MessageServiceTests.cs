using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;
using FluentAssertions;

using Starter.Data.Entities;

namespace Starter.Data.Tests.Services
{
    /// <summary>
    /// Tests for the MessageService class
    /// </summary>
    public class MessageServiceTests: TestsBase
    {
        [Test]
        public void MessageConsumerService_NewInstance_Successful()
        {
            MessageService.Should().NotBeNull();
            LoggerFactoryMock.Verify();
        }

        [Test]
        public async Task OnDataReceived_Successful()
        {
            await MessageService.StartAsync(new CancellationToken());

            MessageBrokerMock.Raise(MessageCommand.Create);

            LoggerFactoryMock.LoggerMock.Verify();
            MessageConsumerMock.Verify();
        }

        [Test]
        public async Task StartAsync_Successful()
        {
            await MessageService.StartAsync(new CancellationToken());
            
            LoggerFactoryMock.LoggerMock.Verify();
            MessageBrokerMock.VerifyRegister();
        }

        [Test]
        public async Task StopAsync_Successful()
        {
            await MessageService.StopAsync(new CancellationToken());
            
            LoggerFactoryMock.LoggerMock.Verify();
        }
    }
}