using aniliberty.Api.Data.Releases;
using aniliberty.Pages.Helpers;
using aniliberty.Pages.Models;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace aniliberty.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class CatalogPage : Page
{
    public CatalogViewModel ViewModel { get; }

    public CatalogPage()
    {
        InitializeComponent();

        ViewModel = new();
        ViewModel.LayoutChanged += OnLayoutChanged;
    }

    private void OnLayoutChanged(object? sender, LayoutKind layoutKind)
    {
        if (layoutKind == LayoutKind.List)
        {
            CatalogItemsView.Layout = new StackLayout();
        }
        else // Grid
        {
            CatalogItemsView.Layout = new UniformGridLayout
            {
                MinItemWidth = 200,             // Минимальная ширина элемента
                MinItemHeight = 250,            // Минимальная высота элемента
                ItemsStretch = UniformGridLayoutItemsStretch.Fill, // Растягивать элементы
                MaximumRowsOrColumns = 5,        // Количество колонок
            };
        }
    }

    private void OnItemTapped(object sender, TappedRoutedEventArgs e)
    {
        // Если клик пришёл от кнопки (или её дочернего элемента) – игнорируем
        if (e.OriginalSource is FrameworkElement fe && fe.FindParent<Button>() != null)
            return;

        var item = (sender as FrameworkElement)?.DataContext as ReleaseCatalog;
        if (item != null)
            ViewModel.ItemSelectedCommand.Execute(item);
    }
}
