using System.Globalization;

namespace SoftSpot_Hein_Myat_Thu.Converters;

public class UnreadNotiBackgroundCoverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Check if the notification is marked as read
        bool isRead = value is true;

        // Get the current app and theme
        var app = Application.Current;
        bool isDarkMode = app?.RequestedTheme == AppTheme.Dark;

        // If the notification is READ
        if (isRead)
        {
            // In DARK mode....use dark gray background
            if (isDarkMode)
            {
                if (app?.Resources["Gray950"] is Color darkBackground)
                {
                    return darkBackground;
                }

                // fallback color
                return Color.FromArgb("#141414");
            }

            // In LIGHT mode...use white background
            if (app?.Resources["White"] is Color lightBackground)
            {
                return lightBackground;
            }

            // fallback color
            return Colors.White;
        }

        // If the notification is UNREAD

        if (isDarkMode)
        {
            // Dark mode unread color
            return Color.FromArgb("#2A2F3A");
        }
        else
        {
            // Light mode unread color
            return Color.FromArgb("#E8E4F5");
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // This converter is one-way only, so we don't support converting back
        throw new NotImplementedException();
    }
}