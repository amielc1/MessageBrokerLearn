using MessageBroker.Core.Enums;

namespace MessageBroker.Core;

public static class GlobalConfiguration
{
    public static MessageBrokerType BrokerType { get; set; } = MessageBrokerType.RabbitMQ;
}