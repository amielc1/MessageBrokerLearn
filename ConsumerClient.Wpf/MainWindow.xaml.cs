using System.Windows.Data;
using ConsumerClient.Wpf.ViewModel;

namespace ConsumerClient.Wpf;

public partial class MainWindow
{
    private readonly object _locker = new();
    public MainWindow(MessagesViewModel messagesViewModel)
    {
        InitializeComponent();
        DataContext = messagesViewModel;
        var messagesList = messagesViewModel.Messages;
        BindingOperations.EnableCollectionSynchronization(messagesList, _locker);
    }
}