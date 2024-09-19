using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.RssFeeds.RoutingConstants;
using DntSite.Web.Features.Searches.Models;
using DntSite.Web.Features.Searches.Services.Contracts;

namespace DntSite.Web.Features.RssFeeds.Components;

public partial class WhatsNew
{
    private const string MainPageTitle = "تازه چه‌خبر";
    private const int ItemsPerPage = 15;

    private PagedResultModel<LuceneSearchResult>? _posts;

    private string PageTitle
        => string.Create(CultureInfo.InvariantCulture, $"{MainPageTitle}، صفحه: {CurrentPage ?? 1}");

    [Parameter] public int? CurrentPage { set; get; }

    [Inject] internal IFullTextSearchService FullTextSearchService { set; get; } = null!;

    [InjectComponentScoped] internal IIndexedDataExplorerService IndexedDataExplorerService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();

        if (FullTextSearchService.IsDatabaseIndexed)
        {
            await ShowIndexedDataAsync();
        }
    }

    private async Task ShowIndexedDataAsync()
    {
        CurrentPage ??= 1;
        _posts = await IndexedDataExplorerService.GetAllPagedIndexedDataAsync(CurrentPage.Value, ItemsPerPage);
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..RssFeedsBreadCrumbs.DefaultBreadCrumbs]);
}
