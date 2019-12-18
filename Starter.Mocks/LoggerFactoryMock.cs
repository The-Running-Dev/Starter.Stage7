using Moq;
using Microsoft.Extensions.Logging;

namespace Starter.Mocks
{
    /// <summary>
    /// Creates and manages setup for a mocked Logger factory
    /// </summary>
    public class LoggerFactoryMock
    {
        public ILoggerFactory Instance { get; }

        private readonly Mock<ILoggerFactory> _mockLoggerFactory;

        public LoggerMock LoggerMock { get; }

        public LoggerFactoryMock()
        {
            LoggerMock = new LoggerMock();

            _mockLoggerFactory = new Mock<ILoggerFactory>();
            _mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(LoggerMock.Instance).Verifiable();

            Instance = _mockLoggerFactory.Object;
        }

        public void Verify()
        {
            _mockLoggerFactory.Verify(
                factory => factory.CreateLogger(It.IsAny<string>()),
                () => Moq.Times.AtLeast(1));
        }
    }
}