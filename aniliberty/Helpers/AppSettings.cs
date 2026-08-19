using Windows.Storage;

namespace aniliberty.Helpers;

internal class AppSettings
{
    public static ApplicationDataContainer GetSettings()
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        return localSettings;
    }

    public static string GetSession()
    {
        var settings = GetSettings();

        return (string)settings.Values["sessionToken"];
    }

    public static void SetSession(string sessionToken)
    {
        var settings = GetSettings();

        settings.Values["sessionToken"] = sessionToken;
    }

    public static bool IsSessionActive()
    {
        var sessionToken = GetSession();

        return !string.IsNullOrEmpty(sessionToken);
    }

    public static string GetVideoQuality()
    {
        var settings = GetSettings();

        return (string)settings.Values["videoQuality"] ?? "1080";
    }

    public static void SetVideoQuality(string quality)
    {
        var settings = GetSettings();

        settings.Values["videoQuality"] = quality;
    }
}
