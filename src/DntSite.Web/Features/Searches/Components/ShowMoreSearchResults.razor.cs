using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Searches.Models;
using DntSite.Web.Features.Searches.RoutingConstants;
using DntSite.Web.Features.Searches.Services.Contracts;

namespace DntSite.Web.Features.Searches.Components;

public partial class ShowMoreSearchResults
{
    private const int ItemsPerPage = 10;
    private const int MaxItems = 70;
    private const int MaxPageNumber = MaxItems / ItemsPerPage;

    private PagedResultModel<LuceneSearchResult>? _posts;

    private string PageTitle => string.Create(CultureInfo.InvariantCulture, $"نتایج جستجوی «{Term}»");

    [Parameter] public string? Term { get; set; }

    [Parameter] public int? CurrentPage { set; get; }

    [Inject] internal IFullTextSearchService FullTextSearchService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [InjectComponentScoped] internal ISearchItemsService SearchItemsService { set; get; } = null!;

    [InjectComponentScoped] internal IIndexedDataExplorerService IndexedDataExplorerService { set; get; } = null!;

    private string BasePath => "/".CombineUrl(SearchesRoutingConstants.SearchResultsBase, escapeRelativeUrl: false)
        .CombineUrl(Term?.Trim() ?? "", escapeRelativeUrl: true);

    protected override async Task OnInitializedAsync()
    {
        ApplicationState.DoNotLogPageReferrer = true;

        AddBreadCrumbs();

        if (FullTextSearchService.IsDatabaseIndexed)
        {
            await ShowSearchResultsAsync();
        }
    }

    private async Task ShowSearchResultsAsync()
    {
        CurrentPage ??= 1;

        if (string.IsNullOrWhiteSpace(Term) || CurrentPage.Value > MaxPageNumber)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        await SearchItemsService.SaveSearchItemAsync(Term);

        _posts = await IndexedDataExplorerService.FindAllPagedIndexedDataAsync(Term.Trim(), MaxItems, CurrentPage.Value,
            ItemsPerPage);
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.Add(SearchesBreadCrumbs.GetBreadCrumb(
            string.Create(CultureInfo.InvariantCulture, $"{PageTitle}، صفحه: {CurrentPage ?? 1}"), BasePath));
}
