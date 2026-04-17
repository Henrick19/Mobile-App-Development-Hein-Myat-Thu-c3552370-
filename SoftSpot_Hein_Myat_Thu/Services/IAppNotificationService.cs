using SoftSpot_Hein_Myat_Thu.Models;

namespace SoftSpot_Hein_Myat_Thu.Services;

public interface IAppNotificationService
{
    Task ShowNotification(string title, string message, NotificationType type);
    Task ScheduleNotification(string title, string message, DateTime notifyTime, NotificationType type, int notificationId);
    Task CancelScheduledNotification(int notificationId);

}