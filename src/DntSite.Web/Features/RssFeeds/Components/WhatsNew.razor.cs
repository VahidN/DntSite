using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.RssFeeds.RoutingConstants;

namespace DntSite.Web.Features.RssFeeds.Components;

public partial class WhatsNew
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        AddBreadCrumbs();
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..RssFeedsBreadCrumbs.DefaultBreadCrumbs]);
}
