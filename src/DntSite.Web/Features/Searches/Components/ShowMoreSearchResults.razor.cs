using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Searches.Models;
using DntSite.Web.Features.Searches.RoutingConstants;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Stats.Middlewares.Contracts;

namespace DntSite.Web.Features.Searches.Components;

[DoNotLogReferrer]
public partial class ShowMoreSearchResults
{
    private const int ItemsPerPage = 10;
    private const int MaxItems = 1000;

    private PagedResultModel<LuceneSearchResult>? _posts;

    private string PageTitle
        => string.Create(CultureInfo.InvariantCulture, $"نتایج جستجوی «{Term}»، صفحه: {CurrentPage ?? 1}");

    [Parameter] public string? Term { get; set; }

    [Parameter] public int? CurrentPage { set; get; }

    [Inject] internal IFullTextSearchService FullTextSearchService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [InjectComponentScoped] internal ISearchItemsService SearchItemsService { set; get; } = null!;

    [InjectComponentScoped] internal IIndexedDataExplorerService IndexedDataExplorerService { set; get; } = null!;

    private string BasePath => "/".CombineUrl(SearchesRoutingConstants.SearchResultsBase)
        .CombineUrl(Uri.EscapeDataString(Term?.Trim() ?? ""));

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();

        if (FullTextSearchService.IsDatabaseIndexed)
        {
            await ShowSearchResultsAsync();
        }
    }

    private async Task ShowSearchResultsAsync()
    {
        CurrentPage ??= 1;

        if (string.IsNullOrWhiteSpace(Term))
        {
            _posts = new PagedResultModel<LuceneSearchResult>();

            return;
        }

        await SearchItemsService.AddSearchItemAsync(Term);

        _posts = await IndexedDataExplorerService.FindAllPagedIndexedDataAsync(Term.Trim(), MaxItems, CurrentPage.Value,
            ItemsPerPage);
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.Add(SearchesBreadCrumbs.GetBreadCrumb(PageTitle, BasePath));
}
