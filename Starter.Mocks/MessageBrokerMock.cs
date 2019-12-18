using System.Threading.Tasks;

using Moq;

using Starter.Data.Entities;
using Starter.Data.Services;

namespace Starter.Mocks
{
    /// <summary>
    /// Creates and manages setup for a mocked Message broker
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageBrokerMock<T>
    {
        private readonly Mock<IMessageBroker<T>> _broker;

        public IMessageBroker<T> Instance { get; }

        public MessageBrokerMock()
        {
            _broker = new Mock<IMessageBroker<T>>();

            _broker.Setup((x) => x.Register()).Verifiable();

            _broker.Setup((x) => x.Send(It.IsAny<Message<T>>()))
                .Returns(Task.CompletedTask).Verifiable();

            Instance = _broker.Object;
        }

        public void Verify(MessageCommand command)
        {
            _broker.Verify(
                x => x.Send(It.Is((Message<T> message) => message.Command == command)),
                Moq.Times.Exactly(1));
        }
    }
}