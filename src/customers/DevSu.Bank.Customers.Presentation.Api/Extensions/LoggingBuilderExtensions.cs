namespace DevSu.Bank.Customers.Presentation.Api.Extensions
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder SetMinimumLevel(this ILoggingBuilder loggingBuilder, bool isDebugMode)
        {
            return loggingBuilder.SetMinimumLevel(isDebugMode ? LogLevel.Debug : LogLevel.Error);
        }
    }
}
