using System;

using Microsoft.Extensions.Logging;

using Starter.Data.Entities;
using Starter.Framework.Clients;

namespace Starter.Data.Consumers
{
    /// <summary>
    /// Implements the message consumer
    /// </summary>
    public class MessageConsumer<T> : IMessageConsumer<T> where T: IEntity
    {
        private readonly IApiClient _apiClient;

        private readonly ILogger _logger;

        public MessageConsumer(IApiClient apiClient, ILogger logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        /// <summary>
        /// Consumes the message from the message broker
        /// </summary>
        /// <param name="message"></param>
        public void Consume(Message<T> message)
        {
            _logger.Log(LogLevel.Information, $"{message.Command}, {message.Type}, {message}");

            switch (message.Command)
            {
                case MessageCommand.Create:
                    _apiClient.Create(message.Entity);

                    break;
                case MessageCommand.Update:
                    _apiClient.Update(message.Entity);

                    break;
                case MessageCommand.Delete:
                    _apiClient.Delete(message.Entity.Id);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}