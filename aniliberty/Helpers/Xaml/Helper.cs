using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace aniliberty.Helpers.Xaml;

internal class Helper
{
    // Helper method to look into the control's template
    public static T? FindNamedChild<T>(DependencyObject parent, string name) where T : DependencyObject
    {
        int childCount = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < childCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is FrameworkElement element && element.Name == name && child is T target)
            {
                return target;
            }

            var result = FindNamedChild<T>(child, name);
            if (result != null) return result;
        }

        return null;
    }
}
