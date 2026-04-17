#if ANDROID
using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;

namespace SoftSpot_Hein_Myat_Thu.Platforms.Android;

public static class AndroidNotificationHelper
{
    private const string ChannelId = "softspot_default";
    private const string ChannelName = "General";
    private const string ChannelDescription = "SoftSpot notifications";

    public static void Show(Context context, string title, string message, int notificationId)
    {
        CreateNotificationChannel(context);

        Intent launchIntent = new Intent(context, typeof(MainActivity)); // open MainActivity when notification is tapped
        launchIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTop);
        launchIntent.PutExtra(AlarmHandler.TitleKey, title);
        launchIntent.PutExtra(AlarmHandler.MessageKey, message);

        PendingIntentFlags flags = Build.VERSION.SdkInt >= BuildVersionCodes.S
            ? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
            : PendingIntentFlags.UpdateCurrent;

        PendingIntent pendingIntent = PendingIntent.GetActivity(context, notificationId, launchIntent, flags); // creating a pending intent to open MainActivity when notification is tapped

        NotificationCompat.Builder builder = new NotificationCompat
            .Builder(context, ChannelId)
            .SetContentTitle(title)
            .SetContentText(message)
            .SetAutoCancel(true)
            .SetSmallIcon(Resource.Mipmap.appicon)
            .SetContentIntent(pendingIntent) 
            .SetPriority((int)NotificationPriority.Default);

        NotificationManagerCompat.From(context).Notify(notificationId, builder.Build());
    }

    public static void CreateNotificationChannel(Context context)
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.O)
        {
            return;
        }

        NotificationChannel channel = new(ChannelId, ChannelName, NotificationImportance.Default)
        {
            Description = ChannelDescription
        };

        NotificationManager manager = (NotificationManager)context.GetSystemService(Context.NotificationService)!;
        manager.CreateNotificationChannel(channel);
    }
}
#endif
