using Microsoft.UI.Xaml.Data;
using System;

namespace aniliberty.Helpers.Xaml;

public partial class LongToDoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is long longVal)
        {
            return (double)longVal;
        }
        return 0.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is double doubleVal)
        {
            return (long)doubleVal;
        }
        return 0L;
    }
}
