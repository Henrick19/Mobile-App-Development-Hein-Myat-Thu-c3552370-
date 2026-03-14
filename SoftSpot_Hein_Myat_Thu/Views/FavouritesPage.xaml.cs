using SoftSpot_Hein_Myat_Thu.Models;    
using SoftSpot_Hein_Myat_Thu.ViewModels;

namespace SoftSpot_Hein_Myat_Thu.Views;

public partial class FavouritesPage : ContentPage
{
    private readonly FavouritesViewModel _vm;

    public FavouritesPage(FavouritesViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm; // bind with the view model, which is injected by the DI container
    }
}