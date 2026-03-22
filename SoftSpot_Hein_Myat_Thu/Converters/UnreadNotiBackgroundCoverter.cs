using System.Globalization;

namespace SoftSpot_Hein_Myat_Thu.Converters;

public class UnreadNotiBackgroundCoverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isRead = false; // default is unread

        if (value is bool)
        {
            isRead = (bool)value;
        }

        if (isRead)
        {
            return Colors.White;
        } 
        else
        {
            if (Application.Current != null) 
            {
                return (Color)Application.Current.Resources["UnreadRowLight"];
            }
        }
        // fallback (just in case)
        return Colors.White;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // We don’t convert color back to bool
        throw new NotImplementedException();
    }
}