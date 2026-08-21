using aniliberty.Api;
using aniliberty.Api.Data.Releases;
using aniliberty.Helpers;
using aniliberty.Helpers.Xaml;
using aniliberty.Pages;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Linq;
using Windows.Media;
using Windows.UI.WindowManagement;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace aniliberty;


/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public string TitleText = "AniLiberty";
    public string SubtitleText = "TEST";
    public Visibility FavMenuVisible => Visibility.Collapsed;

    private readonly Client apiClient = new();

    private Microsoft.UI.Windowing.AppWindow _appWindow;

    public MainWindow()
    {
        InitializeComponent();

        // 1. Extend your custom content into the titlebar
        SetTitleBar(TitleBar); // TitleBar is the Grid element name in your XAML

        // 2. Get the low-level AppWindow reference
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(
            WindowNative.GetWindowHandle(this)
        );
        _appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

        // 3. Get your root UI element (assuming your root grid is named 'RootLayout')
        FrameworkElement root = Content as FrameworkElement;

        if (root != null)
        {
            // Initial call to set colors based on the startup theme
            TitleBarHelper.ApplyThemeToCaptionButtons(_appWindow, root);

            // Listen for runtime theme toggles (e.g., user clicks a dark/light toggle)
            root.ActualThemeChanged += (sender, args) =>
            {
                TitleBarHelper.ApplyThemeToCaptionButtons(_appWindow, root);
            };
        }

        OverlappedPresenter presenter = OverlappedPresenter.Create();
        presenter.PreferredMinimumWidth = 930;

        AppWindow.SetPresenter(presenter);
    }

    // Wraps a call to rootFrame.Navigate to give the Page a way to know which NavigationRootPage is navigating.
    // Please call this function rather than rootFrame.Navigate to navigate the rootFrame.
    public void Navigate(System.Type pageType, object? targetPageArguments = null, NavigationTransitionInfo? navigationTransitionInfo = null)
    {
        RootFrame.Navigate(pageType, targetPageArguments, navigationTransitionInfo);
    }

    private void TitleBar_BackRequested(TitleBar sender, object args)
    {
        if (RootFrame.CanGoBack)
        {
            RootFrame.GoBack();
        }
    }

    private void TitleBar_PaneToggleRequested(TitleBar sender, object args)
    {
        RootNavigationView.IsPaneOpen = !RootNavigationView.IsPaneOpen;
    }

    private void RootNavigationView_Loaded(object sender, RoutedEventArgs e)
    {
        // Add handler for ContentFrame navigation.
        RootFrame.Navigated += On_Navigated;

        // RootNavigationView doesn't load any page by default, so load home page.
        RootNavigationView.SelectedItem = RootNavigationView.MenuItems[0];
        // If navigation occurs on SelectionChanged, this isn't needed.
        // Because we use ItemInvoked to navigate, we need to call Navigate
        // here to load the home page.
        MainNav_Navigate(typeof(RecentPage), new EntranceNavigationTransitionInfo());
    }

    private void RootNavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked == true)
        {
            MainNav_Navigate(typeof(SettingsPage), args.RecommendedNavigationTransitionInfo);
        }
        else if (args.InvokedItemContainer != null)
        {
            var tag = args.InvokedItemContainer.Tag.ToString();
            if (tag == "anilibria.Pages.AccountPage" && !AppSettings.IsSessionActive())
            {
                tag = "anilibria.Pages.LoginPage";
            }

            Debugger.WriteLine($"navigate to page: {tag}", DebuggerCategory.Navigation);
            Debugger.WriteLine($"user session is active: {AppSettings.IsSessionActive()}", DebuggerCategory.Settings);
            System.Type navPageType = System.Type.GetType(tag);
            MainNav_Navigate(navPageType, args.RecommendedNavigationTransitionInfo);
        }
    }

    public void HideNavBar()
    {
        RootNavigationView.IsPaneVisible = false;
    }

    public void ShowNavBar()
    {
        RootNavigationView.IsPaneVisible = true;
    }

    internal bool TryGoBack()
    {
        if (!RootFrame.CanGoBack)
            return false;

        // Don't go back if the nav pane is overlayed.
        if (RootNavigationView.IsPaneOpen &&
            (RootNavigationView.DisplayMode == NavigationViewDisplayMode.Compact ||
             RootNavigationView.DisplayMode == NavigationViewDisplayMode.Minimal))
            return false;

        RootFrame.GoBack();
        return true;
    }

    private void MainNav_Navigate(System.Type navPageType, NavigationTransitionInfo transitionInfo)
    {
        System.Type preNavPageType = RootFrame.CurrentSourcePageType;

        // Only navigate if the selected page isn't currently loaded.
        if (navPageType is not null && !Equals(preNavPageType, navPageType))
        {
            RootFrame.Navigate(navPageType, null, transitionInfo);
        }
    }

    private void On_Navigated(object sender, NavigationEventArgs e)
    {
        RootNavigationView.IsBackEnabled = RootFrame.CanGoBack;

        if (RootFrame.SourcePageType == typeof(SettingsPage))
        {
            // SettingsItem is not part of MainNav.MenuItems, and doesn't have a Tag.
            RootNavigationView.SelectedItem = (NavigationViewItem)RootNavigationView.SettingsItem;
            RootNavigationView.Header = "ѕараметры";
        }
        else if (RootFrame.SourcePageType == typeof(SignInPage))
        {
            RootNavigationView.SelectedItem = RootNavigationView.FooterMenuItems
                        .OfType<NavigationViewItem>()
                        .FirstOrDefault(i => i.Tag.Equals("anilibria.Pages.AccountPage"), null);
            RootNavigationView.Header = "¬ход в аккаунт";
        }
        else if (RootFrame.SourcePageType == typeof(PlayerPage))
        {
            InitializeSmtc();
        }
        else if (RootFrame.SourcePageType != null)
        {
            // Select the nav view item that corresponds to the page being navigated to.
            RootNavigationView.SelectedItem = RootNavigationView.MenuItems
                        .OfType<NavigationViewItem>()
                        .FirstOrDefault(i => i.Tag.Equals(RootFrame.SourcePageType.FullName.ToString()), null) ?? RootNavigationView.FooterMenuItems
                        .OfType<NavigationViewItem>()
                        .FirstOrDefault(i => i.Tag.Equals(RootFrame.SourcePageType.FullName.ToString()), null);

            RootNavigationView.Header = ((NavigationViewItem)RootNavigationView.SelectedItem)?.Content?.ToString();
        }
    }

    private SystemMediaTransportControls _smtc;

    private void InitializeSmtc()
    {
        // Retrieve the HWND (Window Handle) for your WinUI 3 Window
        IntPtr hWnd = WindowNative.GetWindowHandle(this);

        // Get the SMTC instance linked to your Window's handle
        _smtc = SystemMediaTransportControlsInterop.GetForWindow(hWnd);

        // Explicitly enable it and subscribe to user interactions
        _smtc.IsEnabled = true;
        _smtc.ButtonPressed += Smtc_ButtonPressed;

        // Declare which taskbar buttons you want to activate
        _smtc.IsPlayEnabled = true;
        _smtc.IsPauseEnabled = true;
        _smtc.IsNextEnabled = true;
        _smtc.IsPreviousEnabled = true;
    }

    private void UpdateTaskbarMediaDisplay(string title, string artist)
    {
        SystemMediaTransportControlsDisplayUpdater updater = _smtc.DisplayUpdater;
        updater.MusicProperties.Title = title;
        updater.MusicProperties.Artist = artist;

        // Optional: Add thumbnail image
        // updater.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/AlbumArt.jpg"));

        updater.Update();
    }

    // Keep the system updated on the exact playback status
    private void UpdatePlaybackState(MediaPlaybackStatus status)
    {
        _smtc.PlaybackStatus = status; // e.g., MediaPlaybackStatus.Playing or Paused
    }

    private async void Smtc_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
    {
        // Ensure thread safety when touching UI/media elements from a background taskbar hook
        this.DispatcherQueue.TryEnqueue(() =>
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    // Your app code: PlayMedia();
                    UpdatePlaybackState(MediaPlaybackStatus.Playing);
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    // Your app code: PauseMedia();
                    UpdatePlaybackState(MediaPlaybackStatus.Paused);
                    break;
                case SystemMediaTransportControlsButton.Next:
                    // Your app code: NextTrack();
                    break;
            }
        });
    }

    private async void SearchSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
            return;

        var searchText = sender.Text.Trim();
        if (string.IsNullOrEmpty(searchText))
        {
            sender.ItemsSource = null;
            return;
        }

        sender.ItemsSource = await apiClient.Search(searchText);
    }

    private void SearchSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (args.SelectedItem is Release selected)
        {
            sender.Text = ""; // Set the text input to chosen value
            App.MainWindow.Navigate(typeof(ReleasePage), selected.ID);

            Popup? suggestionsPopup = Helper.FindNamedChild<Popup>(sender, "SuggestionsPopup");

            suggestionsPopup?.IsOpen = false;
        }
    }

    private void SearchSuggestBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is AutoSuggestBox autoSuggestBox)
        {
            // Optional: Only show suggestions if there is already text entered
            if (!string.IsNullOrEmpty(autoSuggestBox.Text))
            {
                // Find the internal Popup using VisualTreeHelper
                Popup? suggestionsPopup = Helper.FindNamedChild<Popup>(autoSuggestBox, "SuggestionsPopup");

                suggestionsPopup?.IsOpen = true;
            }
        }
    }

    private void SearchSuggestBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is AutoSuggestBox autoSuggestBox)
        {
            // Find the internal Popup using VisualTreeHelper
            Popup? suggestionsPopup = Helper.FindNamedChild<Popup>(autoSuggestBox, "SuggestionsPopup");

            suggestionsPopup?.IsOpen = false;
        }
    }
}
