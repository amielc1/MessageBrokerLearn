using MessageBroker.Core.Models;
using MessageBroker.Core.Services.Interfaces;
using MessageBroker.Infrastructure.Redis.Builder;
using StackExchange.Redis;

namespace MessageBroker.Infrastructure.Redis;

internal class RedisConsumerAdapter : IConsumerAdapter
{
    private readonly RedisBuilderAdapter _redisBuilder;
    private readonly ISubscriber _consumer;

    public RedisConsumerAdapter(RedisBuilderAdapter redisBuilder)
    {
        _redisBuilder = redisBuilder;
        _consumer = redisBuilder.Build();
    }
        
    public void Subscribe(string topic, Action<ConsumeResultModel> consumeMessageHandler, 
        CancellationToken cancellationToken)
    {
        try
        {
            _consumer.Subscribe(topic, (channel, message) =>
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();

                var consumeResultModel = new ConsumeResultModel(
                    Message: message);
                consumeMessageHandler.Invoke(consumeResultModel);
            });
        }
        catch (Exception ex)
        {
            throw ex switch
            {
                OperationCanceledException => new Exception("Operation was canceled."),
                _ => new Exception(ex.Message)
            };
        }
    }

    public void Dispose()
    {
        _redisBuilder.Dispose();
    }
}