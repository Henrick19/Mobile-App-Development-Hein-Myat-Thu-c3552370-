using System.Globalization;

namespace SoftSpot_Hein_Myat_Thu.Converters;

// converter for noise and crowd level dots for detail page

public class IntToDotsConverter : IValueConverter
{
    private const char Filled = '●';
    private const char Empty = '○';


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int number = 0;

        if (value is int)
        {
            number = (int)value;
        }

        char[] dots = new char[5];

        for (int i = 0; i < dots.Length; i++) 
        {
            if (i < number)
            {
                dots[i] = Filled;
            }
            else
            {
                dots[i] = Empty;
            }
        }
        return new string(dots);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}