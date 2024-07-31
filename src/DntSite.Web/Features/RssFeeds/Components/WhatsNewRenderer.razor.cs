using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.RssFeeds.Services.Contracts;

namespace DntSite.Web.Features.RssFeeds.Components;

public partial class WhatsNewRenderer
{
    private IEnumerable<WhatsNewItemModel>? _posts;

    [InjectComponentScoped] internal IFeedsService FeedsService { set; get; } = null!;

    protected override async Task OnInitializedAsync()
        => _posts = (await FeedsService.GetLatestChangesAsync()).RssItems;
}
