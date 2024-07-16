using MessageBroker.Core.Logger;
using MessageBroker.Core.Services.Interfaces;

namespace MessageBroker.Core.Services;

public class ConsumerService : IConsumerService
{
    private readonly ILoggerAdapter<ConsumerService> _logger;
    private readonly IConsumerAdapter _consumer;
    private readonly IMessageProcessor _messageProcessor;

    public ConsumerService(ILoggerAdapter<ConsumerService> logger, IConsumerAdapter consumer,
        IMessageProcessor messageProcessor)
    {
        _logger = logger;
        _consumer = consumer;
        _messageProcessor = messageProcessor;
    }

    public void Subscribe(string topic, CancellationToken cancellationToken)
    {
        try
        {
            _consumer.Subscribe(topic, consumeMessage =>
            {
                var (success, errorMessage) = _messageProcessor.Process(consumeMessage);
                if (success is false)
                    _logger.LogError($"Fail to process message, {errorMessage}");
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }
}