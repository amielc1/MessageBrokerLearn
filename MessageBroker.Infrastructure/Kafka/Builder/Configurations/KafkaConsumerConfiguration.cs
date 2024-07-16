namespace MessageBroker.Infrastructure.Kafka.Builder.Configurations;

public class KafkaConsumerConfiguration
{
    public string? GroupId { get; set; }
    public string? BootstrapServers { get; set; }
}