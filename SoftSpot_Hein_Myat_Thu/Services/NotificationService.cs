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