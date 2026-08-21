using aniliberty.Api;
using aniliberty.Api.Data.Releases;
using aniliberty.Api.Responses;
using CommunityToolkit.WinUI.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aniliberty.Pages.Helpers;

public class CatalogSource : IIncrementalSource<ReleaseCatalog>
{
    private readonly Client apiClient = new();
    private const int PageSize = 20;

    private bool _hasMore = true; // изначально считаем, что данные есть

    public async Task<IEnumerable<ReleaseCatalog>> GetPagedItemsAsync(int pageIndex, int pageSize = PageSize, CancellationToken cancellationToken = default)
    {
        if (!_hasMore)
        {
            return [];
        }

        PaginatedResponse<ReleaseCatalog> apiResponse = await apiClient.GetCatalogPaginated(pageIndex + 1, pageSize);

        _hasMore = apiResponse.Meta.Pagination.HasMore;

        return apiResponse.List;
    }

    // Метод для сброса состояния (при обновлении списка)
    public void Reset()
    {
        _hasMore = true;
    }
}
