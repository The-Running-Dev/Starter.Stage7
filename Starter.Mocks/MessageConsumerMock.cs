using Moq;

using Starter.Data.Consumers;
using Starter.Data.Entities;

namespace Starter.Mocks
{
    /// <summary>
    /// Creates and manages setup for a mocked Message consumer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageConsumerMock<T> where T: IEntity
    {
        private readonly Mock<IMessageConsumer<T>> _mockConsumer;

        public IMessageConsumer<T> Instance { get; }
        
        public MessageConsumerMock()
        {
            _mockConsumer = new Mock<IMessageConsumer<T>>();

            _mockConsumer.Setup(x => x.Consume(It.IsAny<Message<T>>()))
                .Verifiable();

            Instance = _mockConsumer.Object;
        }

        public void Verify()
        {
            _mockConsumer.Verify();
        }
    }
}