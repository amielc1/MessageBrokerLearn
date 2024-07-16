namespace MessageBroker.Core.Validation.Interfaces;

public interface IMessageValidator
{
    bool Valid(string messageInput);
}