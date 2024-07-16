using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ConsumerClient.Wpf.ConsumerBackgroundServices.Logic;
using ConsumerClient.Wpf.ConsumerBackgroundServices.Model;
using MessageBroker.Core.Logger;
using MessageBroker.Core.Services.Interfaces;

namespace ConsumerClient.Wpf.ViewModel;

public class MessagesViewModel
{
    private readonly ILoggerAdapter<MessagesViewModel> _logger;
    private readonly IMessageProcessor _messageProcessor;

    public MessagesViewModel(ILoggerAdapter<MessagesViewModel> logger,
        IMessageProcessor messageProcessor)
    {
        _logger = logger;
        _messageProcessor = messageProcessor;
        Messages.CollectionChanged += Messages_CollectionChanged;
    }

    private void Messages_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        _logger.LogInformation("Messages collection changed, {e}", e.Action);
    }

    public ObservableCollection<ConsumedMessage> Messages => 
        ((IMessageProcessorMessages) _messageProcessor).Messages;
}

