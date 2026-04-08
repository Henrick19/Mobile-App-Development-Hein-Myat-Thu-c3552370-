using SoftSpot_Hein_Myat_Thu.Models;
using SoftSpot_Hein_Myat_Thu.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace SoftSpot_Hein_Myat_Thu.ViewModels;

public class NotificationsViewModel : BaseViewModel
{
    private readonly IStorageService _storageService;

    private readonly ObservableCollection<Notification> _notifications = new ObservableCollection<Notification>();

    public ObservableCollection<Notification> Notifications
    {
        get { return _notifications; }
    }
    public ICommand LoadCommand { get; }
    public ICommand MarkAllReadCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand ClearAllCommand { get; }

    public NotificationsViewModel(IStorageService storageService)
    {
        _storageService = storageService;

        LoadCommand = new Command(ExecuteLoadCommand);

        MarkAllReadCommand = new Command(ExecuteMarkAllReadCommand);

        DeleteCommand = new Command<Notification>(ExecuteDeleteCommand);

        ClearAllCommand = new Command(ExecuteClearCommand);


    }

    // helper methods

    private async void ExecuteLoadCommand()
    {
        await LoadNotifications();
    }

    private async void ExecuteMarkAllReadCommand()
    {
        await MarkAllReadAsync();
    }

    private async void ExecuteDeleteCommand(Notification notification)
    {
        await DeleteAsync(notification);
    }

    private async void ExecuteClearCommand()
    {
        await ClearAllAsync();
    }
    private async Task LoadNotifications()
    {
        Notifications.Clear();
        var list = await _storageService.GetAllNotificationsAsync();

        // sort newest noti first
        var sortedList = list.OrderByDescending(x => x.CreatedAt);

        foreach (var noti in sortedList)
        {
            Notifications.Add(noti);
        }
    }

    private async Task MarkAllReadAsync()
    {
        var list = await _storageService.GetAllNotificationsAsync();

        bool hasChanges = false;

        foreach (var noti in list)
        {
            if (!noti.IsRead)
            {
                noti.IsRead = true;
                hasChanges = true;
            }
        }
        // only save if there is changes
        if (hasChanges)
        {
            await _storageService.SaveNotificationsAsync(list);
            await LoadNotifications(); // refresh UI
        }
    }

    private async Task DeleteAsync(Notification? notification)
    {
        if (notification == null)
            return;

        var list = await _storageService.GetAllNotificationsAsync();

        // Remove from storage list
        list.RemoveAll(n => n.Id == notification.Id);

        // Save updated list
        await _storageService.SaveNotificationsAsync(list);

        // Remove from UI
        Notifications.Remove(notification);
    }

    private async Task ClearAllAsync()
    {
        var list = await _storageService.GetAllNotificationsAsync();

        if (list.Count == 0)
            return;

        list.Clear();

        await _storageService.SaveNotificationsAsync(list);

        Notifications.Clear(); // clear UI
    }
}