using System;
using Starter.Data.Entities;

namespace Starter.Data.Consumers
{
    /// <summary>
    /// Defines the contract for the message consumer
    /// </summary>
    public interface IMessageConsumer<T>: IDisposable where T: IEntity
    {
        void Consume(Message<T> message);
    }
}