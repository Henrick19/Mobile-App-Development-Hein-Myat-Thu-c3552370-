using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SoftSpot_Hein_Myat_Thu.ViewModels;

public class BaseViewModel : INotifyPropertyChanged // implements the INotifyPropertyChanged interface, which allows the view model to notify the UI when a property value changes
{
    public event PropertyChangedEventHandler? PropertyChanged; // event that is raised when a property value changes

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // this method will be used in the viiew models' setters
    protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string? propertyName = null) // generic method that sets the value of a property and raises the PropertyChanged event if the value has changed
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;


        backingStore = value;
        OnPropertyChanged(propertyName);
        return true;
    }

}