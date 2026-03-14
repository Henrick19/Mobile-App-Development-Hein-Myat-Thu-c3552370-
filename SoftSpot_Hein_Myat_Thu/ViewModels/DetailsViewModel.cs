using SoftSpot_Hein_Myat_Thu.Models;
using SoftSpot_Hein_Myat_Thu.Services;
using System.Windows.Input;

namespace SoftSpot_Hein_Myat_Thu.ViewModels;

public class DetailsViewModel : BaseViewModel
{
    private readonly IPlaceService _placeService;

    private Place _selectedPlace;

    public Place SelectedPlace
    {
        get
        {
            return _selectedPlace;
        }

        set
        {
            SetProperty(ref _selectedPlace, value);
        }

    }

    public DetailsViewModel(IPlaceService placeService)
    {
        _placeService = placeService;
    }
}