using SoftSpot_Hein_Myat_Thu.ViewModels;

namespace SoftSpot_Hein_Myat_Thu.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(".."); // like folder structure, .. means go back to previous page
    }
}