using System;

using Moq;
using Microsoft.Extensions.Logging;

namespace Starter.Mocks
{
    /// <summary>
    /// Creates and manages setup for a mocked Logger
    /// </summary>
    public class LoggerMock
    {
        public ILogger Instance { get; }

        private readonly Mock<ILogger> _mockLogger;

        public LoggerMock()
        {
            _mockLogger = new Mock<ILogger>();
            _mockLogger.Setup(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>())).Verifiable();

            Instance = _mockLogger.Object;
        }

        public void Verify()
        {
            _mockLogger.Verify(
                logger => logger.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()),
                () => Moq.Times.AtLeast(1));
        }
    }
}