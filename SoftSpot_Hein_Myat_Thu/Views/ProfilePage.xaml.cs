
using SoftSpot_Hein_Myat_Thu.ViewModels;

namespace SoftSpot_Hein_Myat_Thu.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModel _vm;

    public ProfilePage(ProfileViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.LoadCommand.Execute(null);
    }

    private async void OnFavouritesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Favourites"); // absolute route to fav page
    }

    private async void OnNotificationsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NotificationsPage));
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }
}