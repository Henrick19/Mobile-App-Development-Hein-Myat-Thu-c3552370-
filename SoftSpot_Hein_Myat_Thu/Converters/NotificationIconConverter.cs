using SoftSpot_Hein_Myat_Thu.Models;
using System.Globalization;

namespace SoftSpot_Hein_Myat_Thu.Converters;

public class NotificationIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is NotificationType type)
        {
            if (type == NotificationType.NewPlaceAlert)
            {
                return "📍";
            }
            else
            {
                return "🔔";
            }
        }
        return "🔔";

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // We don’t convert color back to bool
        throw new NotImplementedException();
    }
}