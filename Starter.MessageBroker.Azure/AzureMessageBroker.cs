using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.ServiceBus;

using Starter.Data.Entities;
using Starter.Data.Services;
using Starter.Framework.Entities;
using Starter.Framework.Extensions;

namespace Starter.MessageBroker.Azure
{
    /// <summary>
    /// Implements IMessageBroker in Azure Service Bus
    /// </summary>
    public class AzureMessageBroker<T> : IMessageBroker<T>
    {
        public event EventHandler<Message<T>> DataReceived;

        private readonly IQueueClient _queueClient;

        public AzureMessageBroker(ISettings settings)
        {
            _queueClient = new QueueClient(settings.ServiceBusConnectionString, settings.ServiceBusQueue);
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

        public void Receive()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(async (Message rawMessage, CancellationToken token) =>
            {
                var message = rawMessage.Body.FromJsonBytes<Message<T>>();

                DataReceived?.Invoke(this, message);

                await _queueClient.CompleteAsync(rawMessage.SystemProperties.LockToken);
            }, messageHandlerOptions);
        }

        public void Stop()
        {
            _queueClient.CloseAsync();
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