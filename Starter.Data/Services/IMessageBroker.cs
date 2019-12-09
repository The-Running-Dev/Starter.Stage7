using System;
using System.Threading.Tasks;

using Starter.Data.Entities;

namespace Starter.Data.Services
{
    /// <summary>
    /// Defines the contract for the message broker
    /// </summary>
    public interface IMessageBroker<T>: IDisposable
    {
        event EventHandler<Message<T>> DataReceived;

        Task Send(Message<T> entity);

        void Receive();
    }
}