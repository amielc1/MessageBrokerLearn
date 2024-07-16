using MessageBroker.Core.Models;

namespace MessageBroker.Core.Services.Interfaces;

public interface IConsumerAdapter : IDisposable
{
    void Subscribe(string topic, Action<ConsumeResultModel> consumeMessageHandler, CancellationToken cancellationToken);
}