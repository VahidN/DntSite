using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.RssFeeds.Services.Contracts;
using DntSite.Web.Features.Searches.Models;
using DntSite.Web.Features.Searches.ModelsMappings;

namespace DntSite.Web.Features.RssFeeds.Components;

public partial class WhatsNewFeedRenderer
{
    private PagedResultModel<LuceneSearchResult>? _posts;

    [InjectComponentScoped] internal IFeedsService FeedsService { set; get; } = null!;

    protected override async Task OnInitializedAsync() => await ShowRssItemsAsync();

    private async Task ShowRssItemsAsync()
    {
        var rssItems = (await FeedsService.GetLatestChangesAsync(false)).RssItems ?? [];

        _posts = new PagedResultModel<LuceneSearchResult>
        {
            Data = rssItems.Select(item => item.MapToLuceneSearchResult()).ToList()
        };
    }
}
