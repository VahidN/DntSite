using DntSite.Web.Features.Searches.Models;
using DntSite.Web.Features.Searches.RoutingConstants;
using DntSite.Web.Features.Searches.Services.Contracts;

namespace DntSite.Web.Features.Searches.Components;

public partial class ShowMoreLikeThis
{
    private IEnumerable<LuceneSearchResult>? _posts;

    private int _totalItems;

    [Inject] internal IFullTextSearchService FullTextSearchService { set; get; } = null!;

    [Parameter] [EditorRequired] public required string DocumentTypeIdHash { set; get; }

    [Parameter] public int MaxItems { set; get; } = 11;

    private string MoreLikeThisUrl => "/"
        .CombineUrl(SearchesRoutingConstants.MoreLikeThisBase, escapeRelativeUrl: false)
        .CombineUrl(DocumentTypeIdHash ?? "", escapeRelativeUrl: true);

    protected override void OnInitialized()
    {
        base.OnInitialized();

        ShowSimilarPosts();
    }

    private void ShowSimilarPosts()
    {
        if (string.IsNullOrWhiteSpace(DocumentTypeIdHash) || MaxItems <= 0)
        {
            return;
        }

        var pagedPosts =
            FullTextSearchService.FindPagedSimilarPosts(DocumentTypeIdHash, MaxItems, pageNumber: 1, MaxItems);

        _posts = pagedPosts.Data
            .Where(x => !string.Equals(x.DocumentTypeIdHash, DocumentTypeIdHash, StringComparison.OrdinalIgnoreCase))
            .DistinctBy(x => x.Url)
            .OrderBy(x => x.ItemType.Value)
            .ThenByDescending(x => x.Score);

        _totalItems = pagedPosts.TotalItems;
    }
}
