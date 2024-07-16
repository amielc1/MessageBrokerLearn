using System.Threading;
using System.Threading.Tasks;
using MessageBroker.Core.Logger;
using MessageBroker.Core.Services.Interfaces;
using Microsoft.Extensions.Hosting;

namespace ConsumerClient.Wpf.ConsumerBackgroundServices;

public class ConsumerService : BackgroundService
{
    private readonly ILoggerAdapter<ConsumerService> _logger;
    private readonly IConsumerService _consumerService;

    public ConsumerService(
        ILoggerAdapter<ConsumerService> logger,
        IConsumerService consumerService)
    {
        _logger = logger;
        _consumerService = consumerService;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ConsumerBackgroundService ExecuteAsync");
        var task = new Task(() =>
        {
            _logger.LogInformation("ConsumerBackgroundService Consumer server task");
            _consumerService.Subscribe("testTopic", stoppingToken);
        });
        task.Start();
        return task;
    }
}