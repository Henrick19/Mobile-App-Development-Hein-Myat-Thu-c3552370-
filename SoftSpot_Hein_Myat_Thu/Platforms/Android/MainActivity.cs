using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.ApplicationModel;

namespace SoftSpot_Hein_Myat_Thu
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _ = RequestNotificationPermissionAsync();
        }

        private static async Task RequestNotificationPermissionAsync()
        {
            await Permissions.RequestAsync<Permissions.PostNotifications>();
        }
    }
}
