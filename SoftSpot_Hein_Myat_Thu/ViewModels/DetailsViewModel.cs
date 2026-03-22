using SoftSpot_Hein_Myat_Thu.Models;
using SoftSpot_Hein_Myat_Thu.Services;
using System.Windows.Input;

namespace SoftSpot_Hein_Myat_Thu.ViewModels;

public class DetailsViewModel : BaseViewModel
{
    private readonly IPlaceService _placeService;
    private readonly IAppNotificationService _notificationService;

    private Place _selectedPlace;
    private bool _notifyWhenQuiet;

    public Place SelectedPlace
    {
        get
        {
            return _selectedPlace;
        }

        set
        {
            SetProperty(ref _selectedPlace, value);

            NotifyWhenQuiet = _selectedPlace?.NotifyWhenQuiet ?? false;
        }

    }

    public bool NotifyWhenQuiet
    {
        get
        {
            return _notifyWhenQuiet;
        }
        set
        {
            SetProperty(ref _notifyWhenQuiet, value);
        }
    }

    public ICommand AddToFavouriteCommand { get; }
    public ICommand NotifyWhenQuietCommand { get; }
    public ICommand OpenMapCommand { get; } 

    public DetailsViewModel(IPlaceService placeService, IAppNotificationService notificationService)
    {
        _placeService = placeService;
        _notificationService = notificationService;

        OpenMapCommand = new Command(OpenMap);

        AddToFavouriteCommand = new Command(AddToFav);

        NotifyWhenQuietCommand = new Command(NotifyQuiet);
       
    }

    private async void OpenMap()
    {
        if (SelectedPlace == null)
        {
            return;
        }
        if (SelectedPlace.LocationLink == null)
        {
            return;
        }
        try
        {
            await Launcher.OpenAsync(SelectedPlace.LocationLink);
        }
        catch
        {
            // ignore errors as in invalid link 
        }
    }

    private async void AddToFav()
    {
        if (SelectedPlace == null)
        {
            return;
        }

        if (SelectedPlace.IsFavorite)
        {
            await _placeService.RemoveFromFavouriteAsync(SelectedPlace);
            await _notificationService.ShowNotification(
                "Removed from favourites",
                $"{SelectedPlace.Name} was removed from your favourites.",
                NotificationType.RemovedFromFavourite);
        }
        else
        {
            await _placeService.AddToFavouriteAsync(SelectedPlace);
            await _notificationService.ShowNotification(
                "Added to favourites",
                $"{SelectedPlace.Name} was saved to your favourites.",
                NotificationType.AddedToFavourite);
        }

        // refresh from storage so UI can updates

        var updatedButton = (await _placeService.GetAllPlacesAsync()).FirstOrDefault(p => p.Id == SelectedPlace.Id || p.Name == SelectedPlace.Name);
        if (updatedButton != null) 
        { 
            SelectedPlace = updatedButton;
        }
    }

    private async void NotifyQuiet()
    {
        if (SelectedPlace == null)
        {
            return;
        }

        bool newValue = !NotifyWhenQuiet;

        await _placeService.SetNotifyWhenQuietAsync(SelectedPlace, newValue); // Save the newvalue into storage via placeservice

        var places = await _placeService.GetAllPlacesAsync(); // refresh data from storage

        Place updated = places.FirstOrDefault(p => p.Id == SelectedPlace.Id || p.Name == SelectedPlace.Name);

        if (updated != null)
        {
            SelectedPlace = updated;
        }
        else
        {
            SelectedPlace.NotifyWhenQuiet = newValue;
            NotifyWhenQuiet = newValue;
        }
    }
}