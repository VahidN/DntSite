using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.RssFeeds.RoutingConstants;
using DntSite.Web.Features.RssFeeds.Services.Contracts;

namespace DntSite.Web.Features.RssFeeds.Components;

public partial class WhatsNew
{
    private IEnumerable<WhatsNewItemModel>? _posts;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [InjectComponentScoped] internal IFeedsService FeedsService { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await ShowNewsItemsAsync();
        AddBreadCrumbs();
    }

    private async Task ShowNewsItemsAsync() => _posts = (await FeedsService.GetLatestChangesAsync()).RssItems;

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..RssFeedsBreadCrumbs.DefaultBreadCrumbs]);
}
