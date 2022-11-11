using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ComicReader.Converters
{
    public class ReverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Equals(value, true) ?
                Visibility.Collapsed :
                Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility visibility)
            {
                return visibility != Visibility.Visible;
            }

            throw new InvalidCastException("ReverseBoolToVisibility.ConvertBack");
        }
    }
}