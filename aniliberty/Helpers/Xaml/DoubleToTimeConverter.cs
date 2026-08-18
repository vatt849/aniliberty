using Microsoft.UI.Xaml.Data;
using System;

namespace aniliberty.Helpers.Xaml;

public partial class DoubleToTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is double doubleValue)
        {
            // Предполагаем, что значение слайдера — это количество тиков (long),
            // преобразованное в double.
            long ticks = (long)doubleValue;
            TimeSpan time = TimeSpan.FromTicks(ticks);
            // Формат: часы:минуты:секунды (например, 01:23:45)
            return time.ToString(@"hh\:mm\:ss");
        }
        return "0:00";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
