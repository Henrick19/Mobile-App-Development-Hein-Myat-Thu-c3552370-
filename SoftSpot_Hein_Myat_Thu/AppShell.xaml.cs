using SoftSpot_Hein_Myat_Thu.Views;
namespace SoftSpot_Hein_Myat_Thu;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(AddPlacePage), typeof(AddPlacePage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(NotificationsPage), typeof(NotificationsPage));
        Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
    }
}
