namespace SoftSpot_Hein_Myat_Thu.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    // key to save data in preferences
    private const string KeyNewPlaceAlert = "NewPlaceAlert";
    private const string KeyPreferredQuietLevel = "PreferredQuietLevel";
    private const string KeyPreferredCrowdLevel = "PreferredCrowdLevel";
    private const string KeyDarkMode = "DarkMode";

    // variables to store the settings values
    private bool _newPlaceAlert;
    private string _preferredCrowdLevel = "N/A";
    private string _preferredQuietLevel = "N/A";
    private bool _darkMode;

    public bool NewPlaceAlert
    {
        get { return _newPlaceAlert; }
        set
        {
            if (SetProperty(ref _newPlaceAlert, value)) // update the value and notify UI
            {
                Preferences.Set(KeyNewPlaceAlert, value); // save the value to preferences
            } 
        }
    }

    public string PreferredCrowdLevel
    {
        get { return _preferredCrowdLevel; }
        set
        {
            if (SetProperty(ref _preferredCrowdLevel, value))
            {
                Preferences.Set(KeyPreferredCrowdLevel, value ?? "N/A"); // if value is null, save "N/A" instead
            }
        }
    }

    public string PreferredQuietLevel
    {
        get { return _preferredQuietLevel; }
        set
        {
            if (SetProperty(ref _preferredQuietLevel, value))
            {
                Preferences.Set(KeyPreferredQuietLevel, value ?? "N/A");
            }
        }
    }

    public bool DarkMode
    {
        get { return _darkMode; }
        set
        {
            if (SetProperty(ref _darkMode, value))
            {
                Preferences.Set(KeyDarkMode, value);

                if (Application.Current != null)
                {
                    Application.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
                }
            }
        }
    }

    // options for crowd and quiet level picters
    public IList<string> CrowdLevelOptions { get; } = new List<string> { "N/A", "Low", "Medium", "High" };
    public IList<string> QuietLevelOptions { get; } = new List<string> { "N/A", "Low", "Medium", "High" };
    
    // constructor
    public SettingsViewModel()
    {
        _newPlaceAlert = Preferences.Get(KeyNewPlaceAlert, false);
        _preferredCrowdLevel = Preferences.Get(KeyPreferredCrowdLevel, "N/A");
        _preferredQuietLevel = Preferences.Get(KeyPreferredQuietLevel, "N/A");
        _darkMode = Preferences.Get(KeyDarkMode, false);
    }
}