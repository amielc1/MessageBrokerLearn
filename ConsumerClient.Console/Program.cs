using ConsumerClient.Console;
using MessageBroker.Core;
using MessageBroker.Core.Enums;
using MessageBroker.Core.Services.Interfaces;
using MessageBroker.Infrastructure.IocContainer;
using MessageBroker.Infrastructure.Kafka.Builder.Configurations;
using MessageBroker.Infrastructure.RabbitMQ.Builder.Configuration;
using MessageBroker.Infrastructure.Redis.Builder.Configuration;
using Microsoft.Extensions.DependencyInjection;

var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) => {
    e.Cancel = true; // prevent the process from terminating.
    cts.Cancel();
};

var services = new ServiceCollection();
services.AddMessageBrokerLoggerServices();

switch (GlobalConfiguration.BrokerType)
{
    case MessageBrokerType.Kafka:
        services.AddMessageBrokerConsumerServicesKafka(new KafkaConsumerConfiguration
        {
            GroupId = "test-consumer-group1",
            BootstrapServers = "localhost:9092"
        });
        break;
    case MessageBrokerType.Redis:
        services.AddMessageBrokerConsumerServicesRedis(new RedisConfiguration
        {
            BootstrapServers = "127.0.0.1:6379"
        });
        break;
    case MessageBrokerType.RabbitMQ:
        services.AddMessageBrokerConsumerServicesRabbitMq(new RabbitMqConfiguration()
        {
            BootstrapServers = "localhost"
        });
        break;
    default:
        throw new ArgumentOutOfRangeException();
}

services.AddScoped<IMessageProcessor, MessageProcessor>();
var serviceProvider = services.BuildServiceProvider();

var consumerService = serviceProvider.GetRequiredService<IConsumerService>();
consumerService.Subscribe("testTopic", cts.Token);
Console.ReadKey();