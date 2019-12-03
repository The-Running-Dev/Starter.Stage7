using System;

using Microsoft.Extensions.Logging;

using Starter.Data.Entities;
using Starter.Framework.Clients;
using Starter.Framework.Extensions;

namespace Starter.Data.Consumers
{
    /// <summary>
    /// Implements the message broker consumer
    /// </summary>
    public class MessageBrokerConsumer : IMessageBrokerConsumer
    {
        private readonly IApiClient _apiClient;
        
        private readonly ILogger _logger;

        public MessageBrokerConsumer(IApiClient apiClient, ILogger logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        /// <summary>
        /// Handles the data received from the message broker
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