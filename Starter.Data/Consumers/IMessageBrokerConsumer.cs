namespace Starter.Data.Consumers
{
    /// <summary>
    /// Defines the contract for the message broker consumer
    /// </summary>
    public interface IMessageBrokerConsumer
    {
        void Consume(string message);
    }
}