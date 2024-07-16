using MessageBroker.Core.Logger;
using Microsoft.Extensions.Logging;

namespace MessageBroker.Infrastructure.Logger;

public class DotnetLogger<T> : ILoggerAdapter<T>
{
    private readonly ILogger<T> _logger;

    public DotnetLogger(ILogger<T> logger)
    {
        _logger = logger;
    }
    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogError(string message, params object[] args)
    {
        _logger.LogError(message, args);
    }
}