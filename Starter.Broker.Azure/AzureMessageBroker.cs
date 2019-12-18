using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.ServiceBus;

using Starter.Data.Entities;
using Starter.Data.Services;
using Starter.Framework.Extensions;
using Starter.Configuration.Entities;

namespace Starter.Broker.Azure
{
    /// <summary>
    /// Implements IMessageBroker in Azure Service Bus
    /// </summary>
    public class AzureMessageBroker<T> : IMessageBroker<T>
    {
        private readonly IQueueClient _queueClient;

        public event EventHandler<Message<T>> DataReceived;

        public AzureMessageBroker(ISettings settings, IQueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public void Register()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(MessageHandler, messageHandlerOptions);
        }

        public async Task Send(Message<T> entity)
        {
            try
            {
                var message = new Message(entity.ToJsonBytes());
                
                await _queueClient.SendAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
                
                throw;
            }
        }

        private async Task MessageHandler(Message rawMessage, CancellationToken token)
        {
            var message = rawMessage.Body.FromJsonBytes<Message<T>>();

            DataReceived?.Invoke(this, message);

            await _queueClient.CompleteAsync(rawMessage.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");

            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }
    }
}