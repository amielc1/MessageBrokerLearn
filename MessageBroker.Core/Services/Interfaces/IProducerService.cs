using MessageBroker.Core.Models;

namespace MessageBroker.Core.Services.Interfaces;

public interface IProducerService
{
    Task<DeliveryResultModel> Produce(MessageModel message, string topic);
}