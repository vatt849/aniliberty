using aniliberty.Helpers;
using FlyleafLib;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.BadgeNotifications;
using System;
using System.IO;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace aniliberty
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        internal static MainWindow MainWindow { get; private set; } = null!;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
            {
                if (e.Exception is TypeInitializationException tie &&
                    tie.TypeName?.Contains("AudioDecoder") == true)
                {
                    Debugger.WriteLine("=== FirstChanceException: Ошибка AudioDecoder ===");
                    Debugger.WriteLine($"Внутреннее исключение: {tie.InnerException}");
                }
                else if (e.Exception is DllNotFoundException or EntryPointNotFoundException or BadImageFormatException)
                {
                    Debugger.WriteLine($"=== FirstChanceException (Нативная DLL): {e.Exception.GetType().Name} ===");
                    Debugger.WriteLine($"Сообщение: {e.Exception.Message}");
                    Debugger.WriteLine($"Стек:\n{e.Exception.StackTrace}");
                }
            };

            InitializeComponent();
            UnhandledException += HandleExceptions;

            try
            {

                Engine.Start(new EngineConfig()
                {
                    FFmpegPath = Path.Combine(AppContext.BaseDirectory, "Flyleaf", "FFmpeg"),
                    PluginsPath = Path.Combine(AppContext.BaseDirectory, "Flyleaf", "Plugins"),
                    FFmpegLoadProfile = Flyleaf.FFmpeg.LoadProfile.All,
#if RELEASE
                    FFmpegLogLevel      = Flyleaf.FFmpeg.LogLevel.Quiet,
                    LogLevel            = LogLevel.Quiet,

#else
                    FFmpegLogLevel = Flyleaf.FFmpeg.LogLevel.Debug,
                    LogLevel = LogLevel.Debug,
                    LogOutput = ":debug",
                    //LogOutput = ":console",
                    //LogOutput = AppContext.BaseDirectory + "flyleaf.log",
#endif

                    UIRefresh = false,    // Required for Activity, BufferedDuration, Stats in combination with Config.Player.Stats = true
                    UIRefreshInterval = 250,      // How often (in ms) to notify the UI
                    //UICurTimePerSecond = true,     // Whether to notify UI for CurTime only when it's second changed or by UIRefreshInterval
                });
            }
            catch (Exception ex)
            {
                //Debugger.WriteLine(Utils.GetFolderPath(":FFmpeg"));
                Debugger.WriteLine(ex.ToString(), DebuggerCategory.App);
            }
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            MainWindow = new MainWindow();
            MainWindow.Activate();

            MainWindow.AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            MainWindow.AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

            MainWindow.Closed += (s, e) =>
            {
                BadgeNotificationManager.Current.ClearBadge();
            };
        }

        private void DebugSettings_BindingFailed(object sender, BindingFailedEventArgs e)
        {
            // Ignore the exception from NonExistentProperty in BindingPage.xaml, 
            // as the sample code intentionally includes a binding failure.
            if (e.Message.Contains("NonExistentProperty"))
            {
                return;
            }

            throw new Exception($"A debug binding failed: " + e.Message);
        }

        /// <summary>
        /// Prevents the app from crashing when a exception gets thrown and notifies the user.
        /// </summary>
        /// <param name="sender">The app as an object.</param>
        /// <param name="e">Details about the exception.</param>
        private void HandleExceptions(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true; //Don't crash the app.

            //Create the notification.
            var notification = new AppNotificationBuilder()
                .AddText("An exception was thrown.")
                .AddText($"Type: {e.Exception.GetType()}")
                .AddText($"Message: {e.Message}\r\n" +
                         $"HResult: {e.Exception.HResult}")
                .BuildNotification();

            //Show the notification
            AppNotificationManager.Default.Show(notification);
        }
    }
}
