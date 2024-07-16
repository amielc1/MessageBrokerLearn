using MessageBroker.Core.Validation.Interfaces;

namespace MessageBroker.Core.Validation;

public class StringMessageValidator : IMessageValidator
{
    public bool Valid(string messageInput)
    {
        return string.IsNullOrWhiteSpace(messageInput) is false;
    }
}