using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Stats.Entities;
using DntSite.Web.Features.Stats.RoutingConstants;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.RoutingConstants;

namespace DntSite.Web.Features.Stats.Components;

public partial class ShowOnlineVisitorsList
{
    private const int ItemsPerPage = 15;
    private PagedResultModel<SiteUrl>? _items;

    [Inject] public IProtectionProviderService ProtectionProvider { set; get; } = null!;

    [InjectComponentScoped] internal ISiteUrlsService SiteUrlsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public string? DeleteId { set; get; }

    [Parameter] public string? CategoryName { set; get; }

    private bool IsSpider => string.Equals(CategoryName, b: "spider", StringComparison.OrdinalIgnoreCase);

    private string PageTitle => string.Create(CultureInfo.InvariantCulture,
            $"{(IsSpider ? StatsBreadCrumbs.OnlineSpiders.Title : StatsBreadCrumbs.OnlineVisitors.Title)}")
        .ToPersianNumbers();

    private string BasePath
        => IsSpider ? StatsRoutingConstants.OnlineSpiderVisitorsUrl : StatsRoutingConstants.OnlineVisitors;

    protected override async Task OnInitializedAsync()
    {
        await TryDeleteItemAsync();
        await ShowResultsAsync();
        AddBreadCrumbs();
    }

    private async Task TryDeleteItemAsync()
    {
        if (!string.IsNullOrWhiteSpace(DeleteId) && ApplicationState.IsCurrentUserAdmin)
        {
            ApplicationState.DoNotLogPageReferrer = true;
            await SiteUrlsService.RemoveSiteUrlAsync(DeleteId.ToInt());
        }
    }

    private async Task ShowResultsAsync()
    {
        CurrentPage ??= 1;
        _items = await SiteUrlsService.GetPagedSiteUrlsAsync(CurrentPage.Value - 1, ItemsPerPage, IsSpider);
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..StatsBreadCrumbs.SiteStatsBreadCrumbs]);

    private string GetUserUrl(string friendlyName)
        => UserProfilesRoutingConstants.Users.CombineUrl(friendlyName, escapeRelativeUrl: true);
}
