using Plugin.LocalNotification;
using SoftSpot_Hein_Myat_Thu.Models;


namespace SoftSpot_Hein_Myat_Thu.Services;

public class NotificationService : IAppNotificationService
{
    private readonly IStorageService _storageService;
    public NotificationService(IStorageService storageService)
    {
        _storageService = storageService;
    }

    // method to show a notification 
    public async Task ShowNotification(string title, string message, NotificationType type)
    {
        NotificationRequest request = new NotificationRequest(); // create request via Plugin.LocalNotification
        request.Title = title;
        request.Description = message;

        if (LocalNotificationCenter.Current != null)
        {
            await LocalNotificationCenter.Current.Show(request);
        }
        await SaveNewNotificationAsync(title, message, type);
    }

    // method to schedule a notification 
    public async Task ScheduleNotification(string title, string message, DateTime notifyTime, NotificationType type, int notificationId)
    {
        NotificationRequest request = new NotificationRequest
        {
            NotificationId = notificationId,
            Title = title,
            Description = message,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = notifyTime
            }
        };

        if (LocalNotificationCenter.Current != null)
        {
            await LocalNotificationCenter.Current.Show(request);
        }

        await SaveNewNotificationAsync(title, message, type);
    }

    // method to cancel a scheduled notification
    public Task CancelScheduledNotification(int notificationId) // method is not async because LocalNotificationCenter.Current.Cancel is not async
    {
        LocalNotificationCenter.Current?.Cancel(notificationId);
        return Task.CompletedTask; // return completed task since this method is not async
    }

    private async Task SaveNewNotificationAsync(string title, string message, NotificationType type)
    {
        var notiList = await _storageService.GetAllNotificationsAsync();

        Notification newNotification = new Notification();

        newNotification.Id = Guid.NewGuid().ToString();
        newNotification.Title = title;
        newNotification.Message = message;
        newNotification.CreatedAt = DateTime.Now;
        newNotification.IsRead = false;
        newNotification.Type = type;

        // add new notification to the list
        notiList.Add(newNotification);
        // save the updated noti list to the storage
        await _storageService.SaveNotificationsAsync(notiList);
    }
}