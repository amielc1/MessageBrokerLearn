using MessageBroker.Core.Models;

namespace MessageBroker.Core.Services.Interfaces;

public interface IMessageProcessor
{
    (bool success, string errorMessage) Process(ConsumeResultModel message);
}