using Confluent.Kafka;
using MessageBroker.Infrastructure.Kafka.Builder.Configurations;

namespace MessageBroker.Infrastructure.Kafka.Builder;

public class KafkaConsumerBuilderAdapter 
{
    private readonly KafkaConsumerConfiguration _configuration;

    public KafkaConsumerBuilderAdapter(KafkaConsumerConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IConsumer<Ignore, string> Build()
    {
        var conf = new ConsumerConfig
        {
            GroupId = _configuration.GroupId,
            BootstrapServers = _configuration.BootstrapServers,
            // Note: The AutoOffsetReset property determines the start offset in the event
            // there are not yet any committed offsets for the consumer group for the
            // topic/partitions of interest. By default, offsets are committed
            // automatically, so in this example, consumption will only start from the
            // earliest message in the topic 'my-topic' the first time you run the program.
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        return new ConsumerBuilder<Ignore, string>(conf).Build();
    }

}