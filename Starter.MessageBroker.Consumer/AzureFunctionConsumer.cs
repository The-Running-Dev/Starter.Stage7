using System;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using Starter.Data.Consumers;

namespace Starter.MessageBroker.Consumer.Azure
{
    /// <summary>
    /// Implements The message broker consumer as Azure function
    /// </summary>
    [ServiceBusAccount("ServiceBusConnectionString")]
    public class AzureFunctionConsumer
    {
        private readonly IMessageBrokerConsumer _messageBrokerConsumer;

        private readonly ILogger<MessageBrokerConsumer> _logger;

        public AzureFunctionConsumer(IMessageBrokerConsumer messageBrokerConsumer, ILogger<MessageBrokerConsumer> logger)
        {
            _messageBrokerConsumer = messageBrokerConsumer;
            _logger = logger;
        }

        [FunctionName("MessageBrokerConsumer")]
        public void Run(
            [ServiceBusTrigger("%ServiceBusQueue%")]string message)
        {
            Console.WriteLine($"Message: {message}");

            _logger.LogInformation($"Message: {message}");

            _messageBrokerConsumer.Consume(message);
        }
    }
}
