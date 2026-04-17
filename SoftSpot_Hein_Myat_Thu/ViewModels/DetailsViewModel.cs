using SoftSpot_Hein_Myat_Thu.Models;
using SoftSpot_Hein_Myat_Thu.Services;
using System.Diagnostics;
using System.Globalization;
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

    // constructor
    public DetailsViewModel(IPlaceService placeService, IAppNotificationService notificationService)
    {
        _placeService = placeService;
        _notificationService = notificationService;

        OpenMapCommand = new Command(OpenMap);

        AddToFavouriteCommand = new Command(AddToFav);

        NotifyWhenQuietCommand = new Command(NotifyQuiet);

    }

    // for map link
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

    // for add/remove fav and show notification
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

        var updatedButton = (await _placeService.GetPlacesAsync()).FirstOrDefault(p => p.Id == SelectedPlace.Id || p.Name == SelectedPlace.Name);
        if (updatedButton != null)
        {
            SelectedPlace = updatedButton;
        }
    }

    // for notify when quiet toggle and show notification immediately if currently quiet or schedule notification for the best time range
    private async void NotifyQuiet()
    {
        if (SelectedPlace == null)
        {
            return;
        }

        bool oldValue = NotifyWhenQuiet;
        bool newValue = !oldValue;

        try
        {
            await _placeService.SetNotifyWhenQuietAsync(SelectedPlace, newValue); // Save the newvalue into storage via placeservice

            int notificationId = GetQuietNotificationId(SelectedPlace);
            if (newValue)
            {
                if (TryGetBestTimeRange(SelectedPlace.BestTime, out DateTime startTime, out DateTime endTime))
                {
                    DateTime todayStart = DateTime.Today.Add(startTime.TimeOfDay); // combine today's date with the time parsed from best time range
                    DateTime todayEnd = DateTime.Today.Add(endTime.TimeOfDay);

                    //if end time is earlier than or equal to start time, it means the quiet time range goes past midnight, so we add 1 day to the end time to make it the next day
                    if (todayEnd <= todayStart) 
                    {
                        todayEnd = todayEnd.AddDays(1);
                    }

                    // if current time is within the quiet time range, show notification immediately
                    if (DateTime.Now >= todayStart && DateTime.Now < todayEnd)
                    {
                        await _notificationService.ShowNotification(
                            "Quiet Time Alert",
                            $"{SelectedPlace.Name} is quiet now.",
                            NotificationType.NotifyWhenQuiet);
                    }
                    else // otherwise, schedule a notification
                    {
                        DateTime scheduledTime = DateTime.Now < todayStart
                            ? todayStart
                            : todayStart.AddDays(1);

                        await _notificationService.ScheduleNotification(
                            "Quiet Time Alert",
                            $"{SelectedPlace.Name} usually becomes quieter around {scheduledTime:h:mm tt}.",
                            scheduledTime,
                            NotificationType.NotifyWhenQuiet,
                            notificationId);
                    }
                }
                else
                {
                    // fallback if best time cannot be parsed from saved data
                    await _notificationService.ShowNotification(
                        "Quiet alert not set",
                        $"Couldn't parse best time for {SelectedPlace.Name}. Please update this place's best time.",
                        NotificationType.NotifyWhenQuiet);
                }
            }
            else // if toggle is turned off, cancel the scheduled notification if any
            {
                await _notificationService.CancelScheduledNotification(notificationId);
            }

            var places = await _placeService.GetPlacesAsync(); // refresh data from storage

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
        catch (Exception ex)
        {
            Debug.WriteLine($"NotifyQuiet failed: {ex}");

            // Restore previous state so UI/storage doesn't stay in a broken toggled value.
            await _placeService.SetNotifyWhenQuietAsync(SelectedPlace, oldValue);
            NotifyWhenQuiet = oldValue;
        }
    }

    // helper methods for notify when quiet
    private static int GetQuietNotificationId(Place place)
    {
        int hash = (place.Id ?? place.Name ?? "quiet-alert").GetHashCode();
        if (hash == int.MinValue)
        {
            return int.MaxValue;
        }

        return Math.Abs(hash);
    }

    // best time parsing 
    private static bool TryGetBestTimeRange(string? bestTime, out DateTime startTime, out DateTime endTime)
    {
        startTime = default;
        endTime = default;

        // Handle empty or N/A
        if (string.IsNullOrWhiteSpace(bestTime) || bestTime.Equals("N/A", StringComparison.OrdinalIgnoreCase))
            return false;

        // Split 
        var parts = bestTime.Split(" - ");
        if (parts.Length != 2)
            return false; 

        // Parse directly
        startTime = DateTime.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
        endTime = DateTime.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);

        return true;
    }
    
}