using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Cyberdeck.Desktop.Converters;

public class ImagePathConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string path || string.IsNullOrWhiteSpace(path))
            return null;

        var fullPath = Path.Combine(AppContext.BaseDirectory, path);

        if (!File.Exists(fullPath))
            return null;

        return new BitmapImage(new Uri(fullPath, UriKind.Absolute));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}