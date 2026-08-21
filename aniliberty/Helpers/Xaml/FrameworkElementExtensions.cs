using Microsoft.UI.Xaml;

namespace aniliberty.Helpers.Xaml;

public static class FrameworkElementExtensions
{
    public static T FindParent<T>(this FrameworkElement element) where T : FrameworkElement
    {
        var parent = element.Parent as FrameworkElement;
        while (parent != null)
        {
            if (parent is T typed) return typed;
            parent = parent.Parent as FrameworkElement;
        }
        return null;
    }
}
