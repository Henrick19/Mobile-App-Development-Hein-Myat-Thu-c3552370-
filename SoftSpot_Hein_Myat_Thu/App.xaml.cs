using SoftSpot_Hein_Myat_Thu.ViewModels;
using SoftSpot_Hein_Myat_Thu.Views;

namespace SoftSpot_Hein_Myat_Thu
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // Match UI to saved preference on cold start (toggle in Settings only updated Current at runtime).
            UserAppTheme = Preferences.Get("DarkMode", false) ? AppTheme.Dark : AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override void OnResume()
        {
            base.OnResume();

            try
            {
                var page = Shell.Current?.CurrentPage;

                if (page?.BindingContext is HomeViewModel homeVm)
                {
                    // App may have been opened via a notification; refresh badge/unread count.
                    _ = homeVm.RefreshUnreadCountAsync();
                }
                else if (page is NotificationsPage && page.BindingContext is NotificationsViewModel notificationsVm)
                {
                    // Ensure the in-app notifications list reflects newly arrived notifications.
                    notificationsVm.LoadCommand.Execute(null);
                }
            }
            catch
            {
                // Never crash the app for a UI refresh.
            }
        }
    }
}