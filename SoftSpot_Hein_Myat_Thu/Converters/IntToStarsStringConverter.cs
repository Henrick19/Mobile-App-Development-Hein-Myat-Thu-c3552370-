using System.Globalization;

namespace SoftSpot_Hein_Myat_Thu.Converters
{
    public class IntToStarsStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int rating = (int)value;

            string stars = "";

            for (int i = 0; i < rating; i++)
            {
                stars += "★";
            }

            for (int i = rating; i < 5; i++)
            {
                stars += "☆";
            }
            return stars;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}