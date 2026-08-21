using aniliberty.Api.Data.Releases;
using aniliberty.Pages.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Collections;
using System;
using System.Threading.Tasks;

namespace aniliberty.Pages.Models;

public partial class CatalogViewModel : ObservableObject
{
    private readonly CatalogSource _source;
    public IncrementalLoadingCollection<CatalogSource, ReleaseCatalog> Items { get; }

    public IRelayCommand<ReleaseCatalog> ItemSelectedCommand { get; }
    public IAsyncRelayCommand LoadMoreCommand { get; }

    // Команда для переключения layout (будет вызываться из View)
    public IRelayCommand<string> SwitchLayoutCommand { get; }

    public CatalogViewModel()
    {
        _source = new();
        Items = new(_source, 15);

        SwitchLayoutCommand = new RelayCommand<string>(OnSwitchLayout);
        ItemSelectedCommand = new RelayCommand<ReleaseCatalog>(OnItemSelected);
        LoadMoreCommand = new AsyncRelayCommand(LoadMoreAsync);

        _ = Items.RefreshAsync();
    }

    // Событие, которое будет подхватывать View для смены Layout
    public event EventHandler<LayoutKind>? LayoutChanged = null;
    private void OnSwitchLayout(string? layoutName)
    {
        var layout = layoutName == "List" ? LayoutKind.List : LayoutKind.Grid;
        LayoutChanged?.Invoke(this, layout);
    }

    private void OnItemSelected(ReleaseCatalog? item)
    {
        if (item is null) return;

        App.MainWindow.Navigate(typeof(ReleasePage), item.ID);
    }

    public async Task RefreshAsync()
    {
        _source.Reset();           // сбрасываем состояние
        await Items.RefreshAsync(); // перезагружаем коллекцию
    }

    // Метод для подгрузки следующей страницы (вызывается автоматически при прокрутке)
    private async Task LoadMoreAsync()
    {
        // Проверяем, есть ли ещё данные (HasMoreItems доступен только для чтения)
        // и не идёт ли уже загрузка
        if (Items.HasMoreItems && !Items.IsLoading)
        {
            await Items.LoadMoreItemsAsync(0); // параметр не используется
        }
    }
}
