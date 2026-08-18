using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.UI;

namespace aniliberty.Helpers;

public static class TitleBarHelper
{
    public static void ApplyThemeToCaptionButtons(AppWindow appWindow, FrameworkElement rootElement)
    {
        if (appWindow == null || appWindow.TitleBar == null) return;

        var titleBar = appWindow.TitleBar;

        // Check the actual requested or system theme applied to the root layout
        if (rootElement.ActualTheme == ElementTheme.Dark)
        {
            // Dark Theme Settings
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.ButtonHoverForegroundColor = Colors.White;
            titleBar.ButtonPressedForegroundColor = Colors.White;

            titleBar.ButtonHoverBackgroundColor = Color.FromArgb(51, 255, 255, 255); // Semi-transparent white
            titleBar.ButtonPressedBackgroundColor = Color.FromArgb(33, 255, 255, 255);
        }
        else
        {
            // Light Theme Settings
            titleBar.ButtonForegroundColor = Colors.Black;
            titleBar.ButtonHoverForegroundColor = Colors.Black;
            titleBar.ButtonPressedForegroundColor = Colors.Black;

            titleBar.ButtonHoverBackgroundColor = Color.FromArgb(26, 0, 0, 0); // Semi-transparent black
            titleBar.ButtonPressedBackgroundColor = Color.FromArgb(13, 0, 0, 0);
        }

        // Always keep the main background of the native buttons transparent 
        // so your custom XAML title bar background shows through perfectly.
        titleBar.ButtonBackgroundColor = Colors.Transparent;
        titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        titleBar.ButtonInactiveForegroundColor = Colors.Gray;
    }
}
