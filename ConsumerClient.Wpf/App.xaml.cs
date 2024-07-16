using System;
using System.Windows;
using ConsumerClient.Wpf.ConsumerBackgroundServices;
using ConsumerClient.Wpf.ConsumerBackgroundServices.Logic;
using ConsumerClient.Wpf.ViewModel;
using MessageBroker.Core;
using MessageBroker.Core.Enums;
using MessageBroker.Core.Services.Interfaces;
using MessageBroker.Infrastructure.IocContainer;
using MessageBroker.Infrastructure.Kafka.Builder.Configurations;
using MessageBroker.Infrastructure.RabbitMQ.Builder.Configuration;
using MessageBroker.Infrastructure.Redis.Builder.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsumerClient.Wpf;

public partial class App
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddDebug();
            })
            .ConfigureServices(services =>
            {
                services.AddMessageBrokerLoggerServices(LoggerType.DotnetLogger);
                switch (GlobalConfiguration.BrokerType)
                {
                    case MessageBrokerType.Kafka:
                        services.AddMessageBrokerConsumerServicesKafka(new KafkaConsumerConfiguration
                        {
                            GroupId = "test-consumer-group2",
                            BootstrapServers = "localhost:9092"
                        });
                        break;
                    case MessageBrokerType.Redis:
                        services.AddMessageBrokerConsumerServicesRedis(new RedisConfiguration()
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
                services.AddScoped<MessagesViewModel>();
                services.AddSingleton<MainWindow>();
                services.AddHostedService<ConsumerService>();
            }).Build();
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        _host.Start();
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}