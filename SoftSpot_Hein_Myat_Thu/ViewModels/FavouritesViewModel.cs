using SoftSpot_Hein_Myat_Thu.Models;
using SoftSpot_Hein_Myat_Thu.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SoftSpot_Hein_Myat_Thu.ViewModels;

public class FavouritesViewModel : BaseViewModel
{
    private readonly IPlaceService _placeService;
    private readonly IAppNotificationService _notificationService;

    private readonly ObservableCollection<Place> _favouritePlaces = new ObservableCollection<Place>();

    public ObservableCollection<Place> FavouritePlaces
    {
        get { return _favouritePlaces; }
    }

    public ICommand LoadCommand { get; }
    public ICommand ClearAllCommand { get; }

    public ICommand PlaceTappedCommand { get; }
    public FavouritesViewModel(IPlaceService placeService, IAppNotificationService notificationService)
    {
        _placeService = placeService;
        _notificationService = notificationService;
        LoadCommand = new Command (ExecuteLoadCommand);
        ClearAllCommand = new Command(ExecuteClearAllCommand);
        PlaceTappedCommand = new Command<Place>(OnPlaceTapped);
    }

    // helper methods
    private async void ExecuteLoadCommand()
    {
        await LoadFav();
    }

    private async void ExecuteClearAllCommand()
    {
        await ClearAll();
    }

    private async Task LoadFav()
    {
        FavouritePlaces.Clear();
        var allPlaces = await _placeService.GetPlacesAsync();
        foreach (var place in allPlaces)
        {
            if (place.IsFavorite)
            {
                FavouritePlaces.Add(place);
            }
        }
    }

    private async Task ClearAll()
    {
        bool hasFav = FavouritePlaces.Count > 0; // for notification, check if there are any fav before clearing

        await _placeService.ClearAllFavouritesAsync();

        await LoadFav(); // reload the list after clearing 

        // for notification
        if (hasFav) 
        { 
            string title = "Favourites Cleared";
            string message = "All places have been removed from your favourites.";

            await _notificationService.ShowNotification(title, message, NotificationType.ClearedFavourites);
        }
    }

    private async void OnPlaceTapped(Place place) // it receives the Place object that was tapped in the UI
    {
        if (place == null)
        {
            return;
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("Place", place);

        await Shell.Current.GoToAsync("DetailsPage", parameters);
    }
}
