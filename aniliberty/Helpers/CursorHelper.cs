using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using System.Reflection;

namespace aniliberty.Helpers;

public static class CursorHelper
{
    public static void SetCursor(UIElement element, InputSystemCursorShape cursorShape)
    {
        var property = typeof(UIElement).GetProperty("ProtectedCursor", BindingFlags.Instance | BindingFlags.NonPublic);
        if (property != null)
        {
            var cursor = InputSystemCursor.Create(cursorShape);
            property.SetValue(element, cursor);
        }
    }
}
