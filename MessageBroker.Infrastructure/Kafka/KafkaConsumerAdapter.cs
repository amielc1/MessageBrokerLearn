using Confluent.Kafka;
using MessageBroker.Core.Models;
using MessageBroker.Core.Services.Interfaces;
using MessageBroker.Infrastructure.Kafka.Builder;

namespace MessageBroker.Infrastructure.Kafka;

internal class KafkaConsumerAdapter : IConsumerAdapter
{
    private readonly IConsumer<Ignore, string> _consumer;

    public KafkaConsumerAdapter(KafkaConsumerBuilderAdapter kafkaConsumerBuilder)
    {
        _consumer = kafkaConsumerBuilder.Build();
    }

    public void Subscribe(string topic, Action<ConsumeResultModel> consumeMessageHandler, 
        CancellationToken cancellationToken)
    {
        _consumer.Subscribe(topic);
        
        while (cancellationToken.IsCancellationRequested is false)
        {
            try
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                var consumeResultModel = new ConsumeResultModel(
                    Message: consumeResult.Message.Value);
                consumeMessageHandler.Invoke(consumeResultModel);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case OperationCanceledException:
                        // Ensure the consumer leaves the group cleanly and final offsets are committed.
                        _consumer.Close();
                        throw new Exception("Operation was canceled.");
                    case ConsumeException exception:
                        throw new Exception(exception.Error.Reason);
                    default:
                        throw new Exception(ex.Message);
                }
            }
        }
    }

    public void Dispose()
    {
        _consumer.Dispose();
    }
}