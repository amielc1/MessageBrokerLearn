namespace MessageBroker.Core.Models;

public record ConsumeResultModel(string Message)
{
    public override string ToString()
    {
        return $"Consumed message '{Message}'";
    }
}