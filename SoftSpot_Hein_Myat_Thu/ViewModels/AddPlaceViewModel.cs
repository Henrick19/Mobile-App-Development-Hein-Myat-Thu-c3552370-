using SoftSpot_Hein_Myat_Thu.Models;
using SoftSpot_Hein_Myat_Thu.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SoftSpot_Hein_Myat_Thu.ViewModels;

public class AddPlaceViewModel : BaseViewModel
{
    private readonly IPlaceService _placeService; 

    // special collection for best time dropdowns
    public ObservableCollection<string> _bestTimeOptions = new ObservableCollection<string>();

    // for rating picker
    public ObservableCollection<int> _ratingOptions = new ObservableCollection<int> { 1, 2, 3, 4, 5};

    public ObservableCollection<string> BestTimeOptions
    {
        get { return _bestTimeOptions; }
    }

    public ObservableCollection<int> RatingOptions
    {
        get { return _ratingOptions; }
    }

    // properties bound to the form fields

    private string _name;
    public string Name { 
        get { return _name; } 

        set
        {
            SetProperty(ref _name, value);
        }
    }

    private string _locationLink;
    public string LocationLink
    {
        get { return _locationLink; }

        set
        {
            SetProperty(ref _locationLink, value);
        }
    }

    private string _type = "N/A"; // default value "N/A"
    public string Type
    {
        get { return _type; }

        set
        {
            SetProperty(ref _type, value);
        }
    }

    // for noise level and crowd level,
    // we can use string properties bound to dropdowns in the UI,
    // and convert them to enums when creating the Place object

    private string _noiseLevel = "Low"; // default value "Low"
    public string NoiseLevel
    {
        get { return _noiseLevel; }

        set
        {
            SetProperty(ref _noiseLevel, value);
        }
    }

    private string _crowdLevel = "Low";
    public string CrowdLevel
    {
        get { return _crowdLevel; }

        set
        {
            SetProperty(ref _crowdLevel, value);
        }
    }

    private bool _hasWifi;
    public bool HasWifi
    {
        get { return _hasWifi; }

        set 
        {
            SetProperty(ref _hasWifi, value);
        }
    }

    private string _bestTimeFrom = "";
    public string BestTimeFrom
    {
        get { return _bestTimeFrom; }

        set
        {
            string newValue;

            if (value == null)
            {
                newValue = "N/A";
            }
            else
            {
                newValue = value;
            }

            SetProperty(ref _bestTimeFrom, newValue);
        }
    }

    private string _bestTimeTo = ""; // default value empty string, which will be converted to "N/A" in the setter if left empty
    public string BestTimeTo
    {
        get { return _bestTimeTo; }

        set
        {
            string newValue;

            if (value == null)
            {
                newValue = "N/A";
            }
            else
            {
                newValue = value;

            }

            SetProperty(ref _bestTimeTo, newValue);
        }
    }

    private string _description;
    public string Description
    {
        get { return _description; }

        set
        {
            SetProperty(ref _description, value); 
        }
    }

    private int _rating = 3; // default value 3
    public int Rating
    {
        get { return _rating; }
        
        set
        {
            SetProperty(ref _rating, value); 
        }
    }

    public ICommand SubmitCommand { get; } // command bound to the "Submit" button in the UI

    public AddPlaceViewModel(IPlaceService placeService)
    {
        _placeService = placeService;
        LoadBestTimeOptions(); // load the best time options for the dropdown
        SubmitCommand = new Command(ExecuteSubmitCommand);
    }

    // helper methods
    private void LoadBestTimeOptions()
    {
        BestTimeOptions.Clear();
        BestTimeOptions.Add(""); // optional: no best time
        foreach (var h in new[] { "6am", "7am", "8am", "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm", "12am" })
            BestTimeOptions.Add(h);
    }

    public async void ExecuteSubmitCommand()
    {
        await AddPlace();
    }

    private async Task AddPlace()
    {
        if (string.IsNullOrWhiteSpace(Name)) 
        {
            return;
        }

        // convert noiselevel string to enum
        SoftSpot_Hein_Myat_Thu.Models.NoiseLevel noiseLevelEnum;
        if (NoiseLevel == "Low")
        {
            noiseLevelEnum = SoftSpot_Hein_Myat_Thu.Models.NoiseLevel.Low;
        }
        else if (NoiseLevel == "Medium")
        {
            noiseLevelEnum = SoftSpot_Hein_Myat_Thu.Models.NoiseLevel.Medium;
        }
        else
        {
            noiseLevelEnum = SoftSpot_Hein_Myat_Thu.Models.NoiseLevel.High;
        }

        // convert crowdlevel string to enum

        SoftSpot_Hein_Myat_Thu.Models.CrowdLevel crowdLevelEnum;
        if (CrowdLevel == "Low")
        {
            crowdLevelEnum = SoftSpot_Hein_Myat_Thu.Models.CrowdLevel.Low;
        }
        else if (CrowdLevel == "Medium")
        {
            crowdLevelEnum = SoftSpot_Hein_Myat_Thu.Models.CrowdLevel.Medium;
        }
        else
        {
            crowdLevelEnum = SoftSpot_Hein_Myat_Thu.Models.CrowdLevel.High;
        }

        // create new place object and fill in the properties
        Place newPlace = new Place
        {
            Name = Name,
            LocationLink = LocationLink,
            Type = Type,
            NoiseLevel = noiseLevelEnum,
            CrowdLevel = crowdLevelEnum,
            HasWifi = HasWifi,
            BestTime = $"{BestTimeFrom} - {BestTimeTo}",
            Description = Description,
            Rating = Rating
        };

        // save the new place using the service
        await _placeService.AddAsync(newPlace);

        // reset the form
        Name = string.Empty;
        LocationLink = string.Empty;
        Type = "N/A";
        NoiseLevel = "Low";
        CrowdLevel = "Low";
        HasWifi = false;
        BestTimeFrom = string.Empty;
        BestTimeTo = string.Empty;
        Description = string.Empty;
        Rating = 3;

    }
}