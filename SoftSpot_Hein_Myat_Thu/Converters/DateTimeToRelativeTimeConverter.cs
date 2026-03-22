using System.Globalization;

namespace SoftSpot_Hein_Myat_Thu.Converters;

public class DateTimeToRelativeTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (!(value is DateTime))
        {
            return "";
        }

        DateTime dateTime = (DateTime)value;

        // calculate tiem difference
        TimeSpan difference = DateTime.Now - dateTime;

        // check how long ago, if less than 1 minute => jsut now
        if (difference.TotalMinutes < 1)
        {
            return "Just now";
        }

        // less than 1 hr
        if (difference.TotalMinutes < 60)
        {
            int minutes = (int)difference.TotalMinutes;
            return minutes + " minutes ago";
        }

        // Less than 1 day
        if (difference.TotalHours < 24)
        {
            int hours = (int)difference.TotalHours;

            if (hours == 1)
            {
                return "1 hour ago";
            }
            else
            {
                return hours + " hours ago";
            }
        }

        // Less than 2 days
        if (difference.TotalDays < 2)
        {
            return "1 day ago";
        }

        // Less than 1 week
        if (difference.TotalDays < 7)
        {
            int days = (int)difference.TotalDays;
            return days + " days ago";
        }

        // Less than 1 month
        if (difference.TotalDays < 30)
        {
            int weeks = (int)(difference.TotalDays / 7);
            return weeks + " weeks ago";
        }

        // More than 1 month → show date
        return dateTime.ToString("MMM d", culture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
    
