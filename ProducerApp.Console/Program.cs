using MessageBroker.Core;
using MessageBroker.Core.Enums;
using MessageBroker.Core.Models;
using MessageBroker.Core.Services.Interfaces;
using MessageBroker.Infrastructure.IocContainer;
using MessageBroker.Infrastructure.Kafka.Builder.Configurations;
using MessageBroker.Infrastructure.RabbitMQ.Builder.Configuration;
using MessageBroker.Infrastructure.Redis.Builder.Configuration;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddMessageBrokerLoggerServices();

switch (GlobalConfiguration.BrokerType)
{
    case MessageBrokerType.Kafka:
        services.AddMessageBrokerProducerServicesKafka(
            new KafkaProducerConfiguration
            {
                BootstrapServers = "localhost:9092"
            });
        break;
    case MessageBrokerType.Redis:
        services.AddMessageBrokerProducerServicesRedis(new RedisConfiguration
        {
            BootstrapServers = "127.0.0.1:6379"
        });
        break;
    case MessageBrokerType.RabbitMQ:
        services.AddMessageBrokerProducerServicesRabbitMq(new RabbitMqConfiguration
        {
            BootstrapServers = "localhost"
        });
        break;
    default:
        throw new ArgumentOutOfRangeException();
}

var serviceProvider = services.BuildServiceProvider();

var producerService = serviceProvider.GetRequiredService<IProducerService>();

var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) => {
    e.Cancel = true; // prevent the process from terminating.
    cts.Cancel();
};

while (cts.IsCancellationRequested is false)
{
    Console.Write("Produce new message: ");
    if(cts.IsCancellationRequested) return;
    var message = new MessageModel(Value: Console.ReadLine() ?? string.Empty);
    await producerService.Produce(message, "testTopic");
}