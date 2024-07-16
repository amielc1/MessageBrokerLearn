using MessageBroker.Core.Models;

namespace MessageBroker.Core.Services.Interfaces;

public interface IProducerAdapter : IDisposable
{
    Task<DeliveryResultModel> ProduceAsync(string topic, MessageModel message);
}