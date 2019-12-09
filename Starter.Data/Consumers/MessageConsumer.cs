using System;

using Microsoft.Extensions.Logging;

using Starter.Data.Entities;
using Starter.Data.Services;
using Starter.Framework.Clients;
using Starter.Framework.Extensions;

namespace Starter.Data.Consumers
{
    /// <summary>
    /// Implements the message consumer
    /// </summary>
    public class MessageConsumer : IMessageConsumer
    {
        private readonly IApiClient _apiClient;

        private readonly IMessageBroker<Cat> _messageBroker;

        private readonly ILogger _logger;

        public MessageConsumer(IMessageBroker<Cat> messageBroker, IApiClient apiClient, ILogger logger)
        {
            _messageBroker = messageBroker;
            _apiClient = apiClient;
            _logger = logger;

            _messageBroker.DataReceived += OnDataReceived;
        }

        public void OnDataReceived(object sender, Message<Cat> message)
        {

        }

        public bool Start()
        {
            _messageBroker.Receive();

            return true;
        }

        public bool Stop()
        {
            _messageBroker.Stop();

            return true;
        }

        /// <summary>
        /// Consumes the message from the message broker
        /// </summary>
        /// <param name="message"></param>
        public void Consume(string message)
        {
            var m = message.FromJson<Message<Cat>>();

            _logger.Log(LogLevel.Information, $"{m.Command}, {m.Type}, {message}");

            switch (m.Command)
            {
                case MessageCommand.Create:
                    _apiClient.Create(m.Entity);

                    break;
                case MessageCommand.Update:
                    _apiClient.Update(m.Entity);

                    break;
                case MessageCommand.Delete:
                    _apiClient.Delete(m.Entity.Id);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}