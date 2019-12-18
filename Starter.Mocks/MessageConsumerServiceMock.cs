using System.Threading;
using System.Threading.Tasks;

using Moq;

using Starter.Data.Entities;
using Starter.Data.Services;

namespace Starter.Mocks
{
    /// <summary>
    /// Creates and manages setup for a mocked Message consumer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageConsumerServiceMock<T>
    {
        private readonly Mock<IMessageService<T>> _service;

        public IMessageService<T> Instance { get; }

        public MessageConsumerServiceMock()
        {
            _service = new Mock<IMessageService<T>>();

            _service.Setup(x => x.StartAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Verifiable();
            
            _service.Setup(x => x.StopAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Verifiable();

            _service.Setup(
                    x =>
                x.OnDataReceived(It.IsAny<object>(), It.IsAny<Message<T>>()))
                .Verifiable();

            Instance = _service.Object;
        }

        public void Verify()
        {
            _service.Verify();
        }
    }
}