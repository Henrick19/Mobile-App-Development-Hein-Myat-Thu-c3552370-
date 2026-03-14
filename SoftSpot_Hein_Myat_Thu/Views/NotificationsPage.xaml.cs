using SoftSpot_Hein_Myat_Thu.ViewModels;

namespace SoftSpot_Hein_Myat_Thu.Views;

public partial class NotificationsPage : ContentPage
{
    private readonly NotificationsViewModel _vm;
    public NotificationsPage(NotificationsViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}