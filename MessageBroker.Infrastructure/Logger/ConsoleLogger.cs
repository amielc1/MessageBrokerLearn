using MessageBroker.Core.Logger;

namespace MessageBroker.Infrastructure.Logger;

public class ConsoleLogger<T> : ILoggerAdapter<T>
{
    public void LogInformation(string message, params object[] args)
    {
        Console.WriteLine($"Information: {message}");
    }

    public void LogError(string message, params object[] args)
    {
        Console.WriteLine($"Error occurred: {message}");
    }
}