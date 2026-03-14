using SoftSpot_Hein_Myat_Thu.ViewModels;

namespace SoftSpot_Hein_Myat_Thu.Views;

public partial class AddPlacePage : ContentPage
{
    public AddPlacePage(AddPlaceViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; // bind with the view model, which is injected by the DI container
    }
    private async void OnBackClicked(object sender, EventArgs e) // event handler for the "Back" button click
    {
        await Shell.Current.GoToAsync("..");
    }
}