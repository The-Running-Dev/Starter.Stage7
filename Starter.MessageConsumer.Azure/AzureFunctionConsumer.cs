using System;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using Starter.Data.Consumers;

namespace Starter.MessageConsumer.Azure
{
    /// <summary>
    /// Implements The message broker consumer as Azure function
    /// </summary>
    [ServiceBusAccount("ServiceBusConnection")]
    public class AzureFunctionConsumer
    {
        private readonly IMessageConsumer _messageConsumer;

        private readonly ILogger<Data.Consumers.MessageConsumer> _logger;

        public AzureFunctionConsumer(IMessageConsumer messageConsumer, ILogger<Data.Consumers.MessageConsumer> logger)
        {
            _messageConsumer = messageConsumer;
            _logger = logger;
        }

        [FunctionName("MessageConsumer")]
        public void Run(
            [ServiceBusTrigger("%ServiceBusQueue%")]string message)
        {
            Console.WriteLine($"Message: {message}");

            _logger.LogInformation($"Message: {message}");

            _messageConsumer.Consume(message);
        }
    }
}