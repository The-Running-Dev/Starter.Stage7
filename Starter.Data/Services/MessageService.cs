using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Starter.Data.Entities;
using Starter.Data.Consumers;
using Starter.Framework.Extensions;

namespace Starter.Data.Services
{
    /// <summary>
    /// Implements the Message Consumer Service as hosted service
    /// </summary>
    public class MessageService<T> : IMessageService<T> where T : IEntity
    {
        private IMessageBroker<T> _broker;

        private IMessageConsumer<T> _consumer;

        private readonly ILogger _logger;

        public MessageService(
            IMessageBroker<T> broker,
            IMessageConsumer<T> consumer,
            ILogger logger)
        {
            _broker = broker;
            _consumer = consumer;
            _logger = logger;

            _broker.DataReceived += OnDataReceived;
        }

        public void OnDataReceived(object sender, Message<T> message)
        {
            _logger.LogInformation($"{message.Command}, {message.Type}, {message.Entity.ToJson()}");

            _consumer.Consume(message);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {nameof(MessageService<T>)}...");

            await Task.Run(() => { _broker.Register(); }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(MessageService<T>)} Stopping...");

            _broker = null;
            _consumer = null;

            return Task.CompletedTask;
        }
    }
}