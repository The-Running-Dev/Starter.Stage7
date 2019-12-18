using System;
using System.Threading.Tasks;

using Starter.Data.Entities;

namespace Starter.Data.Services
{
    /// <summary>
    /// Defines the contract for the message broker
    /// </summary>
    public interface IMessageBroker<T>
    {
        /// <summary>
        /// Handles any received messages
        /// </summary>
        event EventHandler<Message<T>> DataReceived;

        /// <summary>
        /// Sends a message to the queue
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task Send(Message<T> message);

        /// <summary>
        /// Registers the broker to start receiving messages
        /// </summary>
        void Register();
    }
}