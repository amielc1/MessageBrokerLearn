using MessageBroker.Infrastructure.RabbitMQ.Builder.Configuration;
using RabbitMQ.Client;

namespace MessageBroker.Infrastructure.RabbitMQ.Builder
{
    public class RabbitMqBuilderAdapter : IDisposable
    {
        private readonly RabbitMqConfiguration _configuration;
        private IConnection? _connection;
        private IModel? _channel;

        public RabbitMqBuilderAdapter(RabbitMqConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IModel Build()
        {
            var factory = new ConnectionFactory {HostName = _configuration.BootstrapServers};
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            return _channel;
        }

        public void Dispose()
        {
            //_connection?.Close();
            _connection?.Dispose();
            _channel?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
