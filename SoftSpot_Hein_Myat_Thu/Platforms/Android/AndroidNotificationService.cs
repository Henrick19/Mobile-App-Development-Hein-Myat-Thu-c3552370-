#if ANDROID
using Android.App;
using Android.Content;
using Android.OS;
using SoftSpot_Hein_Myat_Thu.Models;
using SoftSpot_Hein_Myat_Thu.Services;
using AppNotification = SoftSpot_Hein_Myat_Thu.Models.Notification;

namespace SoftSpot_Hein_Myat_Thu.Platforms.Android;

public class AndroidNotificationService : IAppNotificationService
{
    private readonly IStorageService _storageService;

    public AndroidNotificationService(IStorageService storageService)
    {
        _storageService = storageService;
        AndroidNotificationHelper.CreateNotificationChannel(Platform.AppContext); // create notification channel
    }

    // method to show an immediate notification and save it to storage
    public async Task ShowNotification(string title, string message, NotificationType type)
    {
        int notificationId = Math.Abs(Guid.NewGuid().GetHashCode());
        AndroidNotificationHelper.Show(Platform.AppContext, title, message, notificationId);
        await SaveNewNotificationAsync(title, message, type);
    }
    // method to schedule a notification and save it to storage
    public async Task ScheduleNotification(string title, string message, DateTime notifyTime, NotificationType type, int notificationId)
    {
        Intent intent = new(Platform.AppContext, typeof(AlarmHandler)); // creating an intent to trigger the AlarmHandler broadcast receiver
        intent.PutExtra(AlarmHandler.TitleKey, title);
        intent.PutExtra(AlarmHandler.MessageKey, message);
        intent.PutExtra(AlarmHandler.NotificationIdKey, notificationId);
        intent.PutExtra(AlarmHandler.TypeKey, (int)type);
        intent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTop);

        PendingIntentFlags flags = Build.VERSION.SdkInt >= BuildVersionCodes.S
            ? PendingIntentFlags.CancelCurrent | PendingIntentFlags.Immutable
            : PendingIntentFlags.CancelCurrent;

        PendingIntent pendingIntent = PendingIntent.GetBroadcast(Platform.AppContext, notificationId, intent, flags); //creating a pending intent to be triggered by the alarm manager at the scheduled time
        long triggerTime = new DateTimeOffset(notifyTime).ToUnixTimeMilliseconds(); 

        AlarmManager? alarmManager = Platform.AppContext.GetSystemService(Context.AlarmService) as AlarmManager;
        if (alarmManager != null)
        {
            try
            {
                bool canUseExact = Build.VERSION.SdkInt < BuildVersionCodes.S || alarmManager.CanScheduleExactAlarms();

                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    if (canUseExact)
                    {
                        alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, triggerTime, pendingIntent);
                    }
                    else
                    {
                        // Prefer user-visible alarm clock API to keep schedule near exact time.
                        AlarmManager.AlarmClockInfo alarmClock = new(triggerTime, pendingIntent);
                        alarmManager.SetAlarmClock(alarmClock, pendingIntent);
                    }
                }
                else
                {
                    alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
                }
            }
            catch
            {
                // Final fallback to avoid crashing if OEM/OS blocks exact scheduling.
                alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
            }
        }

        await SaveNewNotificationAsync(
            "Quiet alert scheduled",
            $"{title} at {notifyTime:h:mm tt}",
            type);
    }

    // method to cancel a scheduled notification
    public Task CancelScheduledNotification(int notificationId)
    {
        Intent intent = new(Platform.AppContext, typeof(AlarmHandler));
        PendingIntentFlags flags = Build.VERSION.SdkInt >= BuildVersionCodes.S
            ? PendingIntentFlags.NoCreate | PendingIntentFlags.Immutable
            : PendingIntentFlags.NoCreate;

        PendingIntent? pendingIntent = PendingIntent.GetBroadcast(Platform.AppContext, notificationId, intent, flags);
        if (pendingIntent != null)
        {
            AlarmManager? alarmManager = Platform.AppContext.GetSystemService(Context.AlarmService) as AlarmManager;
            alarmManager?.Cancel(pendingIntent);
            pendingIntent.Cancel();
        }

        return Task.CompletedTask;
    }

    // method to save a new notification to storage
    private async Task SaveNewNotificationAsync(string title, string message, NotificationType type)
    {
        List<AppNotification> notiList = await _storageService.GetAllNotificationsAsync();

        AppNotification newNotification = new()
        {
            Id = Guid.NewGuid().ToString(),
            Title = title,
            Message = message,
            CreatedAt = DateTime.Now,
            IsRead = false,
            Type = type
        };

        notiList.Add(newNotification);
        await _storageService.SaveNotificationsAsync(notiList);
    }
}
#endif
