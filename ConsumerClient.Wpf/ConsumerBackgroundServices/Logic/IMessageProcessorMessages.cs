using System.Collections.ObjectModel;
using ConsumerClient.Wpf.ConsumerBackgroundServices.Model;

namespace ConsumerClient.Wpf.ConsumerBackgroundServices.Logic;

public interface IMessageProcessorMessages
{
    ObservableCollection<ConsumedMessage> Messages { get; }
}