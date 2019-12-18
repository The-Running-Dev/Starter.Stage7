using System;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Moq;
using Microsoft.Azure.ServiceBus;

namespace Starter.Mocks
{
    /// <summary>
    /// Creates and manages setup for a mocked Queue client
    /// </summary>
    public class QueueClientMock
    {
        public IQueueClient Instance { get; }

        private readonly Mock<IQueueClient> _mockQueueClient;

        public QueueClientMock()
        {
            _mockQueueClient = new Mock<IQueueClient>();

            _mockQueueClient.Setup(x => x.SendAsync(It.IsAny<Message>())).Returns(Task.CompletedTask).Verifiable();

            _mockQueueClient.Setup(x => x.CompleteAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask).Verifiable();

            Instance = _mockQueueClient.Object;
        }

        public void VerifySend()
        {
            Verify((x) => x.SendAsync(It.IsAny<Message>()), 1);
        }

        private void Verify(Expression<Action<IQueueClient>> verifyExpression, int times)
        {
            _mockQueueClient.Verify(verifyExpression, () => Moq.Times.AtLeast(times));
        }
    }
}