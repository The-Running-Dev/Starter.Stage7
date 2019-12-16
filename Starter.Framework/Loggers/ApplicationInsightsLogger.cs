using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.Logging;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

using Starter.Configuration.Entities;

namespace Starter.Framework.Loggers
{
    /// <summary>
    /// Implements the ILogger interface with Application Insights
    /// </summary>
    public class ApplicationInsightsLogger : ILogger, IDisposable
    {
        private readonly TelemetryClient _telemetryClient;

        public ApplicationInsightsLogger(ISettings settings)
        {
            if (settings.ApplicationInsightsInstrumentationKey == null)
            {
                return;
            }

            var config = TelemetryConfiguration.CreateDefault();
            config.InstrumentationKey = settings.ApplicationInsightsInstrumentationKey;
            
            _telemetryClient = new TelemetryClient(config);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Log(logLevel, state.ToString(), exception);
        }

        public void Log(LogLevel logLevel = LogLevel.Trace,
                string message = "",
                Exception exception = null,
                [CallerMemberName] string memberName = "",
                [CallerFilePath] string sourceFilePath = "",
                [CallerLineNumber] int sourceLineNumber = 0)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    _telemetryClient.TrackTrace($"{message} {Environment.NewLine}{exception}",
                        SeverityLevel.Verbose);

                    break;
                case LogLevel.Information:
                    _telemetryClient.TrackTrace($"{message} {Environment.NewLine}{exception}",
                        SeverityLevel.Information);

                    break;
                case LogLevel.Warning:
                    _telemetryClient.TrackTrace($"{message} {Environment.NewLine}{exception}",
                        SeverityLevel.Warning);

                    break;
                case LogLevel.Error:
                    _telemetryClient.TrackException(exception,
                        new Dictionary<string, string>()
                            {{"message", message}, {"SeverityLevel", SeverityLevel.Error.ToString()}});

                    break;
                case LogLevel.Critical:
                    _telemetryClient.TrackException(exception,
                        new Dictionary<string, string>()
                            {{"message", message}, {"SeverityLevel", SeverityLevel.Critical.ToString()}});

                    break;
                case LogLevel.None:
                    break;
            }
        }

        public void Dispose()
        {
            if (_telemetryClient == null)
            {
                return;
            }

            _telemetryClient.Flush();

            System.Threading.Tasks.Task.Delay(5000).Wait();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
}
