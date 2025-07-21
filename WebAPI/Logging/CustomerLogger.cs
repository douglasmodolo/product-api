
namespace WebAPI.Logging
{
    public class CustomerLogger : ILogger
    {
        readonly string loggerName;
        readonly CustomLoggerProviderConfiguration loggerConfig;

        public CustomerLogger(string loggerName, CustomLoggerProviderConfiguration loggerConfig)
        {
            this.loggerName = loggerName;
            this.loggerConfig = loggerConfig;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= loggerConfig.LogLevel;
        }
        
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null; // No scope management implemented
        }        

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string message = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

            WriteLogInTextFile(message);
        }

        private void WriteLogInTextFile(string message)
        {
            string filePath = @"C:\temp\log\log.txt"; // Ensure this directory exists or handle it accordingly

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                try
                {
                    writer.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - {message}");
                    writer.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
