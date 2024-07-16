using MessageBroker.Core.Logger;
using MessageBroker.Core.Models;
using MessageBroker.Core.Services.Interfaces;
using MessageBroker.Core.Validation.Interfaces;

namespace MessageBroker.Core.Services;

public class ProducerService : IProducerService
{
    private readonly ILoggerAdapter<ProducerService> _logger;
    private readonly IProducerAdapter _producer;
    private readonly IMessageValidator _messageValidator;

    public ProducerService(ILoggerAdapter<ProducerService> logger, 
        IProducerAdapter producer, IMessageValidator messageValidator)
    {
        _logger = logger;
        _producer = producer;
        _messageValidator = messageValidator;
    }

    public async Task<DeliveryResultModel> Produce(MessageModel message, string topic)
    {
        try
        {
            if (_messageValidator.Valid(message.Value) is false)
                return new DeliveryResultModel
                {
                    Success = false,
                    ErrorMessage = "Input is not valid."
                };

            var deliveryResult = await _producer.ProduceAsync(topic, message);
            _logger.LogInformation($"Delivery success: {deliveryResult}");

            return deliveryResult;
        }
        catch (Exception e)
        {
            _logger.LogError($"Delivery failed: {e.Message}");
            return new DeliveryResultModel
            {
                Success = false,
                ErrorMessage = "Delivery failed."
            }; 
        }
    }
}