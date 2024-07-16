using MessageBroker.Core.Models;
using MessageBroker.Core.Services.Interfaces;
using MessageBroker.Infrastructure.Redis.Builder;
using StackExchange.Redis;

namespace MessageBroker.Infrastructure.Redis;

internal class RedisProducerAdapter : IProducerAdapter
{
    private readonly RedisBuilderAdapter _redisBuilder;
    private readonly ISubscriber _producer;

    public RedisProducerAdapter(RedisBuilderAdapter redisBuilder)
    {
        _redisBuilder = redisBuilder;
        _producer = redisBuilder.Build();
    }

    public async Task<DeliveryResultModel> ProduceAsync(string topic, MessageModel message)
    {
        try
        {
            var numberOfClientsThatReceivedTheMessage = await _producer.PublishAsync(topic, message.Value);
            return new DeliveryResultModel
            {
                Message = $"Message: '{message.Value}' delivered to {numberOfClientsThatReceivedTheMessage} clients",
                Success = true
            };
        }
        catch (Exception e)
        {
            throw e switch
            {
                _ => new Exception(e.Message)
            };
        }
    }

    public void Dispose()
    {
        _redisBuilder.Dispose();
    }
}