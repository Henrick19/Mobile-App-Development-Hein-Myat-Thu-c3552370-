using SoftSpot_Hein_Myat_Thu.Models;
using SoftSpot_Hein_Myat_Thu.ViewModels;

namespace SoftSpot_Hein_Myat_Thu.Views;

public partial class HomePage : ContentPage 
{
    private readonly HomeViewModel _vm;

    public HomePage(HomeViewModel vm) 
    {
        InitializeComponent();
        BindingContext = _vm = vm; // bind with the view model, which is injected by the DI container
    }

    protected override void OnAppearing()
    {
       base.OnAppearing();
       _vm.LoadCommand.Execute(null);
    }

    private async void OnAddClicked(object sender, EventArgs e) // event handler for the "Add" button click
    {
        await Shell.Current.GoToAsync(nameof(AddPlacePage));
    }

    private async void OnSettingsClicked(object sender, EventArgs e) // event handler for the "Settings" button click
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }

    private async void OnNotificationsClicked(object sender, EventArgs e) // event handler for the "Notifications" button click
    {
        await Shell.Current.GoToAsync(nameof(NotificationsPage));
    }

}