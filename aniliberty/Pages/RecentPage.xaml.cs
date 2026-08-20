using aniliberty.Api;
using aniliberty.Api.Data.Releases;
using aniliberty.Api.Exceptions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace aniliberty.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class RecentPage : Page
{
    public ObservableCollection<Release> CalendarData = new();
    public ObservableCollection<Release> ReleasesData = new();
    private readonly Client apiClient = new();

    public RecentPage()
    {
        InitializeComponent();

        InitializeData();
    }

    private async void InitializeData()
    {
        try
        {
            await LoadCalendar();
            ScheduleView.Visibility = Visibility.Visible;

            await LoadReleases();
            ReleasesView.Visibility = Visibility.Visible;

            LoaderBar.Visibility = Visibility.Collapsed;
        }
        catch (ApiException ex)
        {
            ErrorInfo.Title = "Api error";
            ErrorInfo.Message = string.Format("{0} ({1})", ex.Message, ex.Code);
            ErrorInfo.IsOpen = true;
            ScheduleView.Visibility = Visibility.Collapsed;
            ReleasesView.Visibility = Visibility.Collapsed;

            return;
        }
    }

    private async Task LoadReleases()
    {
        var resp = await apiClient.GetLatestReleases(15);

        foreach (var r in resp)
        {
            Debug.WriteLine($"load calendar: {r.ID}|{r.Title}|{r.PosterUrl}");
            ReleasesData.Add(r);
        }
    }

    private async Task LoadCalendar()
    {
        var resp = await apiClient.GetScheduleNow();

        foreach (var r in resp.Today)
        {
            Debug.WriteLine($"load calendar: {r.Release.ID}|{r.Release.Title}|{r.Release.PosterUrl}");
            CalendarData.Add(r.Release);
        }
    }

    private void ErrorInfo_Closing(InfoBar sender, InfoBarClosingEventArgs args)
    {
        if (args.Reason == InfoBarCloseReason.CloseButton)
        {
            InitializeData();
        }
    }

    private void ScheduleView_ItemInvoked(ItemsView sender, ItemsViewItemInvokedEventArgs args)
    {
        var r = (Release)args.InvokedItem;
        if (r != null)
        {
            App.MainWindow.Navigate(typeof(ReleasePage), r.ID);
        }
    }

    private void ReleasesView_ItemInvoked(ItemsView sender, ItemsViewItemInvokedEventArgs args)
    {
        var r = (Release)args.InvokedItem;
        if (r != null)
        {
            App.MainWindow.Navigate(typeof(ReleasePage), r.ID);
        }
    }

    private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        ReleasesView.Width = e.NewSize.Width - 104;
        ScheduleView.Width = e.NewSize.Width - 104;
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        ReleasesView.Width = ActualWidth - 104;
        ScheduleView.Width = ActualWidth - 104;
    }
    private Point _lastPointSchedule;
    private bool _isDraggingSchedule = false;
    private void ScheduleView_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        var ptr = e.GetCurrentPoint(sender as UIElement);
        if (ptr.Properties.IsLeftButtonPressed)
        {
            _lastPointSchedule = ptr.Position;
            _isDraggingSchedule = true;

            if (sender is UIElement view)
            {
                view.CapturePointer(e.Pointer);
            }
        }
    }

    private void ScheduleView_PointerMoved(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        if (!_isDraggingSchedule)
        {
            return;
        }

        var ptr = e.GetCurrentPoint(sender as UIElement);
        var currentPoint = ptr.Position;

        double deltaX = _lastPointSchedule.X - currentPoint.X;
        double deltaY = _lastPointSchedule.Y - currentPoint.Y;

        // Use the internal ScrollView of the ItemsView
        ScheduleView.ScrollView.ScrollBy(deltaX, deltaY,
                new ScrollingScrollOptions(ScrollingAnimationMode.Disabled));

        _lastPointSchedule = currentPoint;
    }

    private void ScheduleView_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        _isDraggingSchedule = false;


        if (sender is UIElement view)
        {
            view.ReleasePointerCapture(e.Pointer);
        }
    }


    private Point _lastPointReleases;
    private bool _isDraggingReleases = false;
    private void ReleasesView_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        var ptr = e.GetCurrentPoint(sender as UIElement);
        if (ptr.Properties.IsLeftButtonPressed)
        {
            _lastPointReleases = ptr.Position;
            _isDraggingReleases = true;

            if (sender is UIElement view)
            {
                view.CapturePointer(e.Pointer);
            }
        }
    }

    private void ReleasesView_PointerMoved(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        if (!_isDraggingReleases)
        {
            return;
        }

        var ptr = e.GetCurrentPoint(sender as UIElement);
        var currentPoint = ptr.Position;

        double deltaX = _lastPointReleases.X - currentPoint.X;
        double deltaY = _lastPointReleases.Y - currentPoint.Y;

        // Use the internal ScrollView of the ItemsView
        ReleasesView.ScrollView.ScrollBy(deltaX, deltaY,
                new ScrollingScrollOptions(ScrollingAnimationMode.Disabled));

        _lastPointReleases = currentPoint;
    }

    private void ReleasesView_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        _isDraggingReleases = false;

        if (sender is UIElement view)
        {
            view.ReleasePointerCapture(e.Pointer);
        }
    }
}
