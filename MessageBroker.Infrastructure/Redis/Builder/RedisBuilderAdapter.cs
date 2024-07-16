using MessageBroker.Infrastructure.Redis.Builder.Configuration;
using StackExchange.Redis;

namespace MessageBroker.Infrastructure.Redis.Builder;

public class RedisBuilderAdapter : IDisposable
{
    private readonly RedisConfiguration _configuration;
    private ConnectionMultiplexer? _redis;

    public RedisBuilderAdapter(RedisConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ISubscriber Build()
    {
        _redis = ConnectionMultiplexer.Connect(_configuration.BootstrapServers);
        var subscriber = _redis.GetSubscriber();

        if (subscriber is null)
        {
            throw new Exception("Fail to subscribe to Redis");
        }

        return subscriber;
    }

    public void Dispose()
    {
        _redis?.Dispose();
        GC.SuppressFinalize(this);
    }
}