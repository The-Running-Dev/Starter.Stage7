using Starter.Data.Entities;

namespace Starter.Data.Consumers
{
    /// <summary>
    /// Defines the contract for the message consumer
    /// </summary>
    public interface IMessageConsumer<T> where T: IEntity
    {
        /// <summary>
        /// Consumes an individual message
        /// </summary>
        /// <param name="message"></param>
        void Consume(Message<T> message);
    }
}