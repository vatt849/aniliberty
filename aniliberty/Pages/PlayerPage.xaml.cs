using aniliberty.Helpers;
using aniliberty.Pages.Helpers;
using CommunityToolkit.Mvvm.Input;
using FlyleafLib;
using FlyleafLib.Controls.WinUI;
using FlyleafLib.MediaPlayer;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace aniliberty.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PlayerPage : Page
{

    private PlayerData? Data { get; set; }
    private PlaylistData PlaylistData { get; set; } = new();

    public Player Player { get; set; } = new();

    private bool _isDragging;
    private bool _isSeeking;
    private long _targetSeek = -1;
    private readonly DispatcherTimer _seekTimer = new();

    private readonly SymbolIcon iconNormal = new(Symbol.BackToWindow);
    private readonly SymbolIcon iconFullScreen = new(Symbol.FullScreen);
    private readonly FontIcon iconPlay = new() { Glyph = "\uF5B0" };
    private readonly FontIcon iconPause = new() { Glyph = "\uF8AE" };

    public PlayerPage()
    {
        FullScreenContainer.CustomizeFullScreenWindow += FullScreenContainer_CustomizeFullScreenWindow;

        InitializeComponent();
        rootGrid.DataContext = this;

        btnFullScreen.Content = FSC.IsFullScreen ? iconNormal : iconFullScreen;
        btnPlayback.Content = Player.Status == Status.Paused ? iconPlay : iconPause;

        Player.PropertyChanged += Player_PropertyChanged;
        Player.OpenCompleted += Player_OpenCompleted;
        Player.SeekCompleted += Player_SeekCompleted;

        _seekTimer.Interval = TimeSpan.FromSeconds(2);
        _seekTimer.Tick += (s, e) =>
        {
            _seekTimer.Stop();
            _isSeeking = false;
            DispatcherQueue.TryEnqueue(() => timelineSlider.Value = Player.CurTime);
        };

        timelineSlider.AddHandler(
            PointerPressedEvent,
            new PointerEventHandler(TimelineSlider_PointerPressed),
            true
        ); // handledEventsToo = true

        timelineSlider.AddHandler(
            PointerReleasedEvent,
            new PointerEventHandler(TimelineSlider_PointerReleased),
            true
        );
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is not PlayerData pd || pd is null)
        {
            App.MainWindow.TryGoBack();
            return;
        }

        App.MainWindow.HideNavBar();

        Data = pd;

        Player.Playlist.Items.Clear();

        FillPlaylist();

        //Task.Run(() =>
        //{
        //    Thread.Sleep(10); Utils.UIInvoke(() =>
        //    {

        //    });
        //});

        PlayerStart();
    }

    private void Player_SeekCompleted(object? sender, int e)
    {
        Debugger.WriteLine($"seek on: {e}", DebuggerCategory.Player);

        DispatcherQueue.TryEnqueue(() =>
        {
            _isSeeking = false;
            _seekTimer.Stop();
            timelineSlider.Value = Player.CurTime;
        });
    }

    private void Player_OpenCompleted(object? sender, OpenCompletedArgs e)
    {
        if (!e.Success)
        {
            Debugger.WriteLine(e.Error, DebuggerCategory.Player);
            App.MainWindow.TryGoBack();
            return;
        }

        DispatcherQueue.TryEnqueue(() =>
        {
            playerLoader.IsActive = false;
            playerLoader.Visibility = Visibility.Collapsed;
        });

        Debugger.WriteLine($"playing {e.Url}", DebuggerCategory.Player);
        Debugger.WriteLine($"total: {TimeSpan.FromTicks(Player.Duration).TotalSeconds}", DebuggerCategory.Player);
    }

    private void Player_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Player.Status):
                btnPlayback.Content = Player.Status == Status.Paused ? iconPlay : iconPause;

                break;
            case nameof(Player.CurTime):
                if (_isDragging || _isSeeking)
                    return;

                DispatcherQueue.TryEnqueue(() => timelineSlider.Value = Player.CurTime);
                break;
        }
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        App.MainWindow.ShowNavBar();
        Player.Dispose();

        //DispatcherQueue.TryEnqueue(() => EpisodePlayer?.MediaPlayer.Dispose());
    }

    private void FullScreenContainer_CustomizeFullScreenWindow(object? sender, EventArgs e)
    {
        //FullScreenContainer.FSWApp.Title = Title + " (FS)";
        //FullScreenContainer.FSW.Closed += (o, e) => Close();
    }

    private void FillPlaylist()
    {
        PlaylistData = new()
        {
            Playlist = []
        };

        if (Data is null)
        {
            return;
        }

        foreach (var ep in Data.Release?.Episodes ?? [])
        {
            PlaylistData.Playlist.Add(ep);
        }

        PlaylistData.Current = Data.Episode?.Ordinal ?? Data.Release?.Episodes.First().Ordinal ?? 0;
    }

    private void VideoQualityMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not RadioMenuFlyoutItem item || !item.IsChecked || item.GroupName != "VideoQuality")
        {
            return;
        }

        AppSettings.SetVideoQuality((string)item.Tag);
    }
    private void ErrorInfo_Closing(InfoBar sender, InfoBarClosingEventArgs args)
    {
        if (args.Reason == InfoBarCloseReason.CloseButton)
        {
            App.MainWindow.TryGoBack();
        }
    }

    private void PlayerStart()
    {
        playerLoader.IsActive = true;
        playerLoader.Visibility = Visibility.Visible;

        var currentEp = PlaylistData.GetCurrentEpisode();
        if (currentEp is null)
        {
            App.MainWindow.ShowNavBar();
            App.MainWindow.TryGoBack();
            return;
        }

        string quality = AppSettings.GetVideoQuality();
        string? source = quality switch
        {
            "1080" => currentEp.HLS1080,
            "720" => currentEp.HLS720,
            "480" => currentEp.HLS480,
            "torrent" => null,
            _ => null
        };

        Debugger.WriteLine($"current playlist item: {currentEp.ID} - {quality} - {source ?? "null"}");

        if (string.IsNullOrEmpty(source))
        {
            App.MainWindow.TryGoBack();
            return;
        }

        Player.OpenAsync(source);

        //Player.Playlist.Selected.Title = currentEp.Title;

        Player.Play();
    }

    [RelayCommand]
    private void PlayerNext()
    {
        if (!PlaylistData.CanGoNext())
        {
            return;
        }

        PlaylistData.GoNext();

        PlayerStart();
    }

    [RelayCommand]
    private void PlayerPrev()
    {
        if (!PlaylistData.CanGoPrev())
        {
            return;
        }

        PlaylistData.GoPrev();

        PlayerStart();
    }

    private void TimelineSlider_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
    }

    private void TimelineSlider_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        _isDragging = true;
        timelineSlider.CapturePointer(e.Pointer);
    }

    private void TimelineSlider_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        Debugger.WriteLine($"seeking to: {timelineSlider.Value}", DebuggerCategory.Player);

        _isDragging = false;
        timelineSlider.ReleasePointerCapture(e.Pointer);
        SeekTo(timelineSlider.Value);
    }

    private void FSC_FullScreenEnter(object sender, EventArgs e)
    {
        // WinUI bug: keyboard focus

        btnFullScreen.Content = iconNormal;
        App.MainWindow.AppWindow.IsShownInSwitchers = false;
        flyleafHost.KFC.Focus(FocusState.Keyboard);
    }

    private void FSC_FullScreenExit(object sender, EventArgs e)
    {
        btnFullScreen.Content = iconFullScreen;
        App.MainWindow.AppWindow.IsShownInSwitchers = true;
        Task.Run(() => { Thread.Sleep(10); Utils.UIInvoke(() => flyleafHost.KFC.Focus(FocusState.Keyboard)); });
    }

    private void SeekTo(double newTimeline)
    {
        if (Player == null) return;

        long targetTicks = (long)newTimeline;
        _targetSeek = targetTicks;
        _isSeeking = true;

        Debugger.WriteLine($"seek to: {targetTicks}", DebuggerCategory.Player);

        Player.CurTime = targetTicks;

        if (Player.Status == Status.Paused)
            Player.Play();

        _seekTimer.Stop();
        _seekTimer.Start();
    }
}
