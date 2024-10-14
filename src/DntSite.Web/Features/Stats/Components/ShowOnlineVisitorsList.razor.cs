using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.RoutingConstants;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.RoutingConstants;

namespace DntSite.Web.Features.Stats.Components;

public partial class ShowOnlineVisitorsList
{
    private const int ItemsPerPage = 15;
    private PagedResultModel<OnlineVisitorInfoModel>? _items;

    [Inject] internal IOnlineVisitorsService OnlineVisitorsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public string? CategoryName { set; get; }

    private bool IsSpider => string.Equals(CategoryName, b: "spider", StringComparison.OrdinalIgnoreCase);

    private string PageTitle => string.Create(CultureInfo.InvariantCulture,
            $"{(IsSpider ? StatsBreadCrumbs.OnlineSpiders.Title : StatsBreadCrumbs.OnlineVisitors.Title)}، صفحه {CurrentPage ?? 1}")
        .ToPersianNumbers();

    private string BasePath
        => IsSpider ? StatsRoutingConstants.OnlineSpiderVisitorsUrl : StatsRoutingConstants.OnlineVisitors;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        ShowResults();
        AddBreadCrumbs();
    }

    private void ShowResults()
    {
        CurrentPage ??= 1;
        _items = OnlineVisitorsService.GetPagedOnlineVisitorsList(CurrentPage.Value - 1, ItemsPerPage, IsSpider);
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([..StatsBreadCrumbs.OnlineUsersStatsBreadCrumbs]);

    private string GetUserUrl(string friendlyName)
        => UserProfilesRoutingConstants.Users.CombineUrl(friendlyName, escapeRelativeUrl: true);
}
