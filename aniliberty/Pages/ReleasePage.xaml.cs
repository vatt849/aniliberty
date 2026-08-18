using aniliberty.Api;
using aniliberty.Api.Data.Releases;
using aniliberty.Api.Data.Releases.Episodes;
using aniliberty.Api.Exceptions;
using aniliberty.Helpers;
using aniliberty.Pages.Helpers;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace aniliberty.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ReleasePage : Page
{
    internal ReleaseDetail? release;
    internal ObservableCollection<Episode> Episodes = [];

    internal int ViewedEps = 0;
    internal int ViewedPercent = 0;

    public ReleasePage()
    {
        InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is int rID && rID > 0)
        {
            var api = new Client();

            try
            {
                release = await api.GetRelease(rID);

                TitleName.Text = release.Title;
                TitleNameEn.Text = release.Name.English;

                TitleDescription.Text = release.Description;

                TitleImage.Source = new BitmapImage(new Uri(release.PosterUrl));

                FavBtnText.Text = release.InFavorites;
                TitleYear.Text = $"{release.Year} г.";
                TitleType.Text = release.Type.Description;
                TitleStatus.Text = release.IsInProduction || release.IsOngoing ? "В работе / Онгоинг" : "Завершён";

                release.Episodes.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));

                foreach (var item in release.Episodes)
                {
                    Episodes.Add(item);
                }

                if (release.Episodes.Count > 0)
                {
                    var rnd = new Random();

                    var viewedCount = rnd.Next(0, release.Episodes.Count);

                    for (int i = 0; i < viewedCount; i++)
                    {
                        Episodes[i].Viewed = true;
                    }

                    UpdateViewed();

                    Debugger.WriteLine($"episodes viewed: {viewedCount}", DebuggerCategory.App);
                    Debugger.WriteLine($"episodes viewed percent: {ViewedPercent}", DebuggerCategory.App);
                }

                LoaderBar.Visibility = Visibility.Collapsed;
            }
            catch (ApiException ex)
            {
                ErrorInfo.Title = "Api error";
                ErrorInfo.Message = string.Format("{0} ({1})", ex.Message, ex.Code);
                ErrorInfo.IsOpen = true;
                return;
            }
        }
        else
        {
            ErrorInfo.Title = "App error";
            ErrorInfo.Message = "Unknown release";
            ErrorInfo.IsOpen = true;
        }

        base.OnNavigatedTo(e);
    }

    private void ErrorInfo_Closing(InfoBar sender, InfoBarClosingEventArgs args)
    {
        if (args.Reason == InfoBarCloseReason.CloseButton)
        {
            App.MainWindow.TryGoBack();
        }
    }

    internal void UpdateViewed()
    {
        ViewedEps = Episodes.Count(x => x.Viewed);
        ViewedPercent = (int)((float)ViewedEps / release.Episodes.Count * 100);

        if (ViewedEps > 0)
        {
            EpisodesProgressText.Text = $"Просмотрено {ViewedEps} {(ViewedEps == 1 ? "эпизод" : (ViewedEps < 5 ? "эпизода" : "эпизодов"))} из {release.Episodes.Count}";

            PlayBtn.Content = "Продолжить просмотр";
        }
        else
        {
            EpisodesProgressText.Text = "Не просмотрено ни одного эпизода";

            if (Episodes.Count == 0) { PlayBtn.Visibility = Visibility.Collapsed; }
            PlayBtn.Content = "Начать просмотр";
        }

        EpisodesProgress.Value = ViewedPercent;
    }

    private void EpisodesList_ItemClick(object sender, ItemClickEventArgs e)
    {
        var ep = (Episode)e.ClickedItem;
        if (ep != null)
        {
            App.MainWindow.Navigate(typeof(PlayerPage), new PlayerData()
            {
                Release = release,
                Episode = ep,
            });
        }
    }

    private void EpisodeMarkViewed_Click(object sender, RoutedEventArgs e)
    {
        var btn = sender as MenuFlyoutItem;

        int i = Episodes.IndexOf(Episodes.First(x => x.Ordinal.ToString() == btn.Tag.ToString()));
        if (Episodes[i].Viewed)
        {
            return;
        }

        Episodes[i].Viewed = true;

        UpdateViewed();

        if (EpisodesList.FindDescendant<FontIcon>(x => x.Name is "ViewedMark" && x.Tag.ToString() == btn.Tag.ToString()) is FontIcon viewedMark)
        {
            viewedMark.Visibility = Visibility.Visible;
        }

        Debugger.WriteLine($"ep i: {i}, ep viewed: {Episodes[i].Viewed}", DebuggerCategory.App);
    }

    private void EpisodeUnmarkViewed_Click(object sender, RoutedEventArgs e)
    {
        var btn = sender as MenuFlyoutItem;

        int i = Episodes.IndexOf(Episodes.First(x => x.Ordinal.ToString() == btn.Tag.ToString()));
        if (!Episodes[i].Viewed)
        {
            return;
        }

        Episodes[i].Viewed = false;

        UpdateViewed();

        if (EpisodesList.FindDescendant<FontIcon>(x => x.Name is "ViewedMark" && x.Tag.ToString() == btn.Tag.ToString()) is FontIcon viewedMark)
        {
            viewedMark.Visibility = Visibility.Collapsed;
        }

        Debugger.WriteLine($"ep i: {i}, ep viewed: {Episodes[i].Viewed}", DebuggerCategory.App);
    }

    private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        DetailsPanel.Width = e.NewSize.Width - 430;
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        DetailsPanel.Width = ActualWidth - 430;
    }

    private void MainContentFrame_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        // Ширина страницы минус отступы (36 слева + 36 справа = 72)
        double availableWidth = e.NewSize.Width - 72;
        if (availableWidth > 0)
        {
            // Ограничиваем максимальную ширину ContentGrid, чтобы он не расширялся за пределы окна
            ContentGrid.MaxWidth = availableWidth;
            // Дополнительно можно ограничить TitleGrid (хотя ContentGrid уже ограничит его)
            // TitleGrid.MaxWidth = availableWidth;
        }
    }

    private void CopyBtn_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        string textBlockName = button?.Tag as string;
        if (string.IsNullOrEmpty(textBlockName)) return;

        // Находим TextBlock по имени в текущем контексте
        var textBlock = this.FindName(textBlockName) as TextBlock;
        string textToCopy = textBlock?.Text;

        if (!string.IsNullOrEmpty(textToCopy))
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(textToCopy);
            Clipboard.SetContent(dataPackage);
        }
    }
}
