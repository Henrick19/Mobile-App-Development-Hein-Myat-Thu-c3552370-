using SoftSpot_Hein_Myat_Thu.Models;
using SoftSpot_Hein_Myat_Thu.Services;
using System.Collections.ObjectModel;
using System.Windows.Input; 

namespace SoftSpot_Hein_Myat_Thu.ViewModels;

public class HomeViewModel : BaseViewModel 
{
    private readonly IPlaceService _placeService;   // service to manage place data, injected via the constructor
    private readonly IStorageService _storageService; // service to manage data storage, injected via the constructor
    private readonly ObservableCollection<Place> _places = new ObservableCollection<Place>(); // notifies the UI whenever items are added, removed, or changed and 
    private List<Place> _allPlaces = new List<Place>(); // holds all places loaded from the service, used for filtering

    public ObservableCollection<Place> Places // UI binds to this property to display the list of places 
    {
        get { return _places; }
    }

    private string _searchText = string.Empty;
    public string SearchText
    {
        get { return _searchText; } 
        set 
        { 
            if (SetProperty(ref _searchText, value)) 
            {
                ApplyFilters(); // Re-apply filters when search text changes
            }
        }

    }
    // UI filter buttons binding

    private bool _filterQuiet;

    // Property to get/set the filter and apply filters when changed
    public bool FilterQuiet
    {
        get
        {
            return _filterQuiet;
        }
        set
        {
            if (SetProperty(ref _filterQuiet, value)) // value is always the value being assigned to the property
            {
                ApplyFilters();
            }
        }
    }

    private bool _filterCrowd;
    public bool FilterCrowd
    {
        get { return _filterCrowd; }
        set
        {
            if (SetProperty(ref _filterCrowd, value))
            {
                ApplyFilters();
            }
        }
    }

    private bool _filterWifi;
    public bool FilterWifi
    {
        get { return _filterWifi; }
        set
        {
            if (SetProperty(ref _filterWifi, value))
            {
                ApplyFilters();
            }
        }
    }

    // Unread noti count
    private int _unreadNotiCont;
    public int UnreadNotiCont
    {
        get
        {
            return _unreadNotiCont;
        }
        set
        {
            if (SetProperty(ref _unreadNotiCont, value))
            {
                // Tell UI these properties also changed
                OnPropertyChanged(nameof(ShowUnreadBadge));
                OnPropertyChanged(nameof(UnreadBadgeText));
            }
        }
    }

    // helper properties
    public bool ShowUnreadBadge
    {
        get
        {
            return UnreadNotiCont > 0;
        }
    }

    public string UnreadBadgeText
    {
        get
        {
            if (UnreadNotiCont > 99)
                return "99+";

            return UnreadNotiCont.ToString();
        }
    }

    public ICommand LoadCommand { get; } // Command to load places when the page appears

    // Command to toggle the filter when the button is clicked
    public ICommand ToggleQuietFilterCommand { get; }
    public ICommand ToggleCrowdFilterCommand { get; }
    public ICommand ToggleWifiFilterCommand { get; }
    public ICommand PlaceTappedCommand { get; }


    // Constructor with dependency injection for services
    public HomeViewModel(IPlaceService placeService, IStorageService storageService)
    {
        _placeService = placeService;
        _storageService = storageService;

        LoadCommand = new Command(ExecuteLoadCommand);
        ToggleQuietFilterCommand = new Command(ToggleQuietFilter);
        ToggleCrowdFilterCommand = new Command(ToggleCrowdFilter);
        ToggleWifiFilterCommand = new Command(ToggleWifiFilter);
        PlaceTappedCommand = new Command<Place>(OnPlaceTapped); // this commmand will receive a Place object when it runs
    }

    // helper methods for the commands
    private async void ExecuteLoadCommand() { 
        await LoadPlaces();
    }
    private void ToggleQuietFilter() 
    {
        FilterQuiet = !FilterQuiet;
    }

    private void ToggleCrowdFilter()
    {
        FilterCrowd = !FilterCrowd;
    }

    private void ToggleWifiFilter()
    {
        FilterWifi = !FilterWifi;
    }

    private async Task LoadPlaces()
    {
        _allPlaces = await _placeService.GetAllPlacesAsync();

        ApplyFilters();
        await RefreshUnreadCountAsync();
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

    // method to refresh unread count
    internal async Task RefreshUnreadCountAsync()
    {
        var notificationList = await _storageService.GetAllNotificationsAsync();
        int count = 0;
        foreach (var noti in notificationList)
        {
            if (!noti.IsRead)
            {
                count++;
            }
        }
        UnreadNotiCont = count;
    }

    // Method to apply the filter to the places
    private void ApplyFilters()
    {
        // Clear current displayed list
        _places.Clear();

        // Start with all places
        List<Place> filtered = new List<Place>();

        foreach (Place place in _allPlaces)
        {
            bool includePlace = true;
            
            // search filter
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string search = SearchText.Trim();

                if (!place.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                {
                    includePlace = false;

                }
            }

            //  quiet filter
            if (FilterQuiet)
            {
                if (place.NoiseLevel != NoiseLevel.Low)
                {
                    includePlace = false;
                }

            }

            // crowd filter
            if (FilterCrowd)
            {
                if (place.CrowdLevel == CrowdLevel.High)
                {
                    includePlace = false;
                }
               
            }

            // wifi filter
            if (FilterWifi)
            {
                if (!place.HasWifi)
                {
                    includePlace = false;
                }
            }

            // If the place meets all criteria, add it to the filtered list
            if (includePlace)
            {
                filtered.Add(place);
            }
        }

        // Add filtered places to the ObservableCollection
        foreach (var place in filtered)
        {
            _places.Add(place);
        }
    } 

}
