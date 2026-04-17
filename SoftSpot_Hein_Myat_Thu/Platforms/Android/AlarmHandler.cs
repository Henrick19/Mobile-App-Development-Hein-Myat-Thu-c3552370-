#if ANDROID
using Android.Content;
using SoftSpot_Hein_Myat_Thu.Models;
using SoftSpot_Hein_Myat_Thu.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftSpot_Hein_Myat_Thu.Platforms.Android;

[BroadcastReceiver(Enabled = true, Exported = false, Label = "SoftSpot Alarm Receiver")]
public class AlarmHandler : BroadcastReceiver
{
    public const string TitleKey = "title";
    public const string MessageKey = "message";
    public const string NotificationIdKey = "notificationId";
    public const string TypeKey = "notificationType";

    public override void OnReceive(Context? context, Intent? intent)
    {
        if (context == null || intent?.Extras == null)
        {
            return;
        }

        string title = intent.GetStringExtra(TitleKey) ?? "SoftSpot";
        string message = intent.GetStringExtra(MessageKey) ?? string.Empty;
        int notificationId = intent.GetIntExtra(NotificationIdKey, Math.Abs(Guid.NewGuid().GetHashCode()));
        NotificationType type = (NotificationType)intent.GetIntExtra(TypeKey, (int)NotificationType.NotifyWhenQuiet);

        AndroidNotificationHelper.Show(context, title, message, notificationId);

        BroadcastReceiver.PendingResult pendingResult = GoAsync(); // Get a PendingResult to keep the receiver alive while we do async work
        _ = Task.Run(async () => // run the async work on a background thread
        {
            // create a new notification and save it to storage
            try
            {
                StorageService storageService = new();
                List<Notification> notiList = await storageService.GetAllNotificationsAsync();
                notiList.Add(new Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = title,
                    Message = message,
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    Type = type
                });
                await storageService.SaveNotificationsAsync(notiList);
            }
            finally
            {
                pendingResult.Finish(); // signal that tells the system we're done with the async work and it can release the receiver
            }
        });
    }
}
#endif
