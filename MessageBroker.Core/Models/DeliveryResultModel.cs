namespace MessageBroker.Core.Models;

public class DeliveryResultModel
{
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    
    public override string ToString()
    {
        return $"Delivered '{Message}'";
    }
}