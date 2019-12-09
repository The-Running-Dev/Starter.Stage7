using Starter.Data.Entities;

namespace Starter.Data.Consumers
{
    /// <summary>
    /// Defines the contract for the message broker consumer
    /// </summary>
    public interface IMessageConsumer
    {
        void OnDataReceived(object sender, Message<Cat> message);

        void Consume(string message);

        bool Start();

        bool Stop();
    }
}