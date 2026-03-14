using SoftSpot_Hein_Myat_Thu.Models;
using SoftSpot_Hein_Myat_Thu.ViewModels;

namespace SoftSpot_Hein_Myat_Thu.Views;

[QueryProperty(nameof(TappedPlace), "Place")]
public partial class DetailsPage : ContentPage
{
    private readonly DetailsViewModel _vm;

    public Place TappedPlace
    {
        set 
        {
            _vm.SelectedPlace = value;
        }
        
    }

    public DetailsPage(DetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm; 
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}