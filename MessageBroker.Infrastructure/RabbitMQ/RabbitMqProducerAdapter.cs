using System.Text;
using MessageBroker.Core.Models;
using MessageBroker.Core.Services.Interfaces;
using MessageBroker.Infrastructure.RabbitMQ.Builder;
using RabbitMQ.Client;

namespace MessageBroker.Infrastructure.RabbitMQ
{
    public class RabbitMqProducerAdapter : IProducerAdapter
    {
        private readonly RabbitMqBuilderAdapter _rabbitMqBuilder;
        private readonly IModel _channel;

        public RabbitMqProducerAdapter(RabbitMqBuilderAdapter rabbitMqBuilder)
        {
            _rabbitMqBuilder = rabbitMqBuilder;
            _channel = rabbitMqBuilder.Build();
        }

        public Task<DeliveryResultModel> ProduceAsync(string topic, MessageModel message)
        {
            try
            {
                _channel.ExchangeDeclare(exchange: topic, type: ExchangeType.Fanout);

                var messageBytes = Encoding.UTF8.GetBytes(message.Value);
                _channel.BasicPublish(exchange: topic, routingKey: topic, basicProperties: null, messageBytes);

                return Task.FromResult(new DeliveryResultModel
                {
                    Message = $"Message: '{message.Value}' delivered to clients",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                throw ex switch
                {
                    _ => new Exception(ex.Message)
                };
            }
        }

        public void Dispose()
        {
            _rabbitMqBuilder.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
