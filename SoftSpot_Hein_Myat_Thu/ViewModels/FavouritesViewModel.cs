using SoftSpot_Hein_Myat_Thu.Models;
using SoftSpot_Hein_Myat_Thu.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SoftSpot_Hein_Myat_Thu.ViewModels;

public class FavouritesViewModel : BaseViewModel
{
    private readonly IPlaceService _placeService;
    public FavouritesViewModel(IPlaceService placeService)
    {
        _placeService = placeService;
    }
}
