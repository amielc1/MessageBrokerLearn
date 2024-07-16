using System.Collections.ObjectModel;
using ConsumerClient.Wpf.ConsumerBackgroundServices.Model;
using MessageBroker.Core.Logger;
using MessageBroker.Core.Models;
using MessageBroker.Core.Services.Interfaces;

namespace ConsumerClient.Wpf.ConsumerBackgroundServices.Logic;

public class MessageProcessor : IMessageProcessor, IMessageProcessorMessages
{
    private readonly ILoggerAdapter<MessageProcessor> _logger;
    public ObservableCollection<ConsumedMessage> Messages { get; } = new();

    public MessageProcessor(ILoggerAdapter<MessageProcessor> logger)
    {
        _logger = logger;
    }

    public (bool success, string errorMessage) Process(ConsumeResultModel message)
    {
        _logger.LogInformation($"New message arrived! - {message}");
        Messages.Add(new ConsumedMessage(message.ToString()));
        _logger.LogInformation($"Message consumed! - {message}");
        return (success: true, errorMessage: string.Empty);
    }
}