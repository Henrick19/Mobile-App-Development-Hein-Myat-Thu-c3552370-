using System.Globalization;
using System.Runtime.InteropServices.ObjectiveC;

namespace SoftSpot_Hein_Myat_Thu.Converters;

public class BoolToWifiStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string isWifi = "";

        if (value is true)
        {
            isWifi = "Available";
        }
        else { isWifi = "No"; }
        return isWifi;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}