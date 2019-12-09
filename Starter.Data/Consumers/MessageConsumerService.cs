using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Starter.Data.Entities;
using Starter.Data.Services;
using Starter.Framework.Extensions;

namespace Starter.Data.Consumers
{
    public class MessageConsumerService : IMessageConsumerService, IDisposable
    {
        private IMessageBroker<Cat> _broker;

        private IMessageConsumer<Cat> _consumer;

        private readonly ILogger<MessageConsumerService> _logger;

        public MessageConsumerService(
            IMessageBroker<Cat> broker,
            IMessageConsumer<Cat> consumer,
            ILogger<MessageConsumerService> logger)
        {
            _broker = broker;
            _consumer = consumer;
            _logger = logger;

            _broker.DataReceived += OnDataReceived;
        }

        public void OnDataReceived(object sender, Message<Cat> message)
        {
            _logger.LogInformation($"{message.Command}, {message.Type}, {message.Entity.ToJson()}");

            _consumer.Consume(message);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {nameof(MessageConsumerService)}...");

            await Task.Run(() => { _broker.Receive(); });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(MessageConsumerService)} Stopped...");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _broker.Dispose();
            _broker = null;

            _consumer.Dispose();
            _consumer = null;
        }
    }
}