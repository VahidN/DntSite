using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Searches.Models;
using DntSite.Web.Features.Searches.RoutingConstants;
using DntSite.Web.Features.Searches.Services.Contracts;

namespace DntSite.Web.Features.Searches.Components;

public partial class ShowMoreLikeThisSearchResults
{
    private const int ItemsPerPage = 10;
    private const int MaxItems = 70;

    private LuceneSearchResult? _luceneDocument;

    private string? _pageTitle;

    private PagedResultModel<LuceneSearchResult>? _posts;

    [Parameter] public string? DocumentTypeIdHash { get; set; }

    [Parameter] public int? CurrentPage { set; get; }

    [Inject] internal IFullTextSearchService FullTextSearchService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [InjectComponentScoped] internal IIndexedDataExplorerService IndexedDataExplorerService { set; get; } = null!;

    private string BasePath => "/".CombineUrl(SearchesRoutingConstants.MoreLikeThisBase, escapeRelativeUrl: false)
        .CombineUrl(DocumentTypeIdHash ?? "", escapeRelativeUrl: true);

    protected override async Task OnInitializedAsync()
    {
        if (FullTextSearchService.IsDatabaseIndexed)
        {
            await ShowSearchResultsAsync();
        }

        AddBreadCrumbs();
    }

    private async Task ShowSearchResultsAsync()
    {
        CurrentPage ??= 1;

        if (string.IsNullOrWhiteSpace(DocumentTypeIdHash))
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _luceneDocument = FullTextSearchService.FindLuceneDocument(DocumentTypeIdHash);

        if (_luceneDocument is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        SetPageTitle();

        _posts = await IndexedDataExplorerService.FindAllMoreLikeThisItemsAsync(DocumentTypeIdHash, MaxItems,
            CurrentPage.Value, ItemsPerPage);
    }

    private void SetPageTitle()
        => _pageTitle = string.Create(CultureInfo.InvariantCulture, $"نتایج مشابه «{_luceneDocument?.OriginalTitle}»");

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            SearchesBreadCrumbs.GetBreadCrumb(_luceneDocument?.OriginalTitle, _luceneDocument?.Url,
                DntBootstrapIcons.BiNewspaper),
            SearchesBreadCrumbs.GetBreadCrumb(
                string.Create(CultureInfo.InvariantCulture, $"{_pageTitle}، صفحه: {CurrentPage ?? 1}"), BasePath)
        ]);
}
