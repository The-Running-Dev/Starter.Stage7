using System;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using Starter.Data.Consumers;
using Starter.Data.Entities;
using Starter.Framework.Extensions;

namespace Starter.MessageConsumer.Azure
{
    /// <summary>
    /// Implements The message consumer as Azure function
    /// </summary>
    [ServiceBusAccount("ServiceBusConnection")]
    public class AzureFunctionConsumer
    {
        private readonly IMessageConsumer<Cat> _consumer;

        private readonly ILogger<IMessageConsumer<Cat>> _logger;

        public AzureFunctionConsumer(IMessageConsumer<Cat> consumer, ILogger<IMessageConsumer<Cat>> logger)
        {
            _consumer = consumer;
            _logger = logger;
        }

        [FunctionName("MessageConsumer")]
        public void Run(
            [ServiceBusTrigger("%ServiceBusQueue%")]string message)
        {
            Console.WriteLine($"Message: {message}");

            _logger.LogInformation($"Message: {message}");

            _consumer.Consume(message.FromJson<Message<Cat>>());
        }
    }
}