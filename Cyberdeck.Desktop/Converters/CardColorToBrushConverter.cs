using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Cyberdeck.Desktop.Converters;

public class CardColorToBrushConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        var color = value.ToString();

        return color switch
        {
            "Red" => "#FF5A5F",
            "Blue" => "#4DB8FF",
            "Green" => "#58FFB2",
            "Yellow" => "#FFD95A",

            _ => Brushes.Gray
        };
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}