using MessageBroker.Core;
using MessageBroker.Core.Enums;
using MessageBroker.Core.Models;
using MessageBroker.Core.Services.Interfaces;
using MessageBroker.Infrastructure.IocContainer;
using MessageBroker.Infrastructure.Kafka.Builder.Configurations;
using MessageBroker.Infrastructure.RabbitMQ.Builder.Configuration;
using MessageBroker.Infrastructure.Redis.Builder.Configuration;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMessageBrokerLoggerServices(LoggerType.DotnetLogger);

switch (GlobalConfiguration.BrokerType)
{
    case MessageBrokerType.Kafka:
        builder.Services.AddMessageBrokerProducerServicesKafka(
            new KafkaProducerConfiguration
            {
                BootstrapServers = "localhost:9092"
            });
        break;
    case MessageBrokerType.Redis:
        builder.Services.AddMessageBrokerProducerServicesRedis(new RedisConfiguration
        {
            BootstrapServers = "127.0.0.1:6379"
        });
        break;
    case MessageBrokerType.RabbitMQ:
        builder.Services.AddMessageBrokerProducerServicesRabbitMq(new RabbitMqConfiguration
        {
            BootstrapServers = "localhost"
        });
        break;
    default:
        throw new ArgumentOutOfRangeException();
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/produceMessage", async ([FromServices] IProducerService producerService, MessageModel message) =>
{
    await producerService.Produce(message, "testTopic");
    return Results.Ok();
}).WithName("ProduceMessage");

app.Run();
