using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Stats.Entities;
using DntSite.Web.Features.Stats.RoutingConstants;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.RoutingConstants;

namespace DntSite.Web.Features.Stats.Components;

public partial class ShowSiteReferrers
{
    private const int ItemsPerPage = 10;

    private PagedResultModel<SiteReferrer>? _items;

    [InjectComponentScoped] internal ISiteReferrersService SiteReferrersService { set; get; } = null!;

    [Inject] public IProtectionProviderService ProtectionProvider { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public string? DeleteId { set; get; }

    protected override async Task OnInitializedAsync()
    {
        if (!string.IsNullOrWhiteSpace(DeleteId))
        {
            await TryDeleteItemAsync(DeleteId.ToInt());

            return;
        }

        await ShowResultsAsync();
        AddBreadCrumbs();
    }

    private async Task TryDeleteItemAsync(int id)
    {
        await SiteReferrersService.RemoveSiteReferrerAsync(id);
        ApplicationState.NavigateTo($"{StatsRoutingConstants.SiteReferrers}#main");
    }

    private async Task ShowResultsAsync()
    {
        CurrentPage ??= 1;
        _items = await SiteReferrersService.GetPagedSiteReferrersAsync(CurrentPage.Value - 1, ItemsPerPage);
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            UserProfilesBreadCrumbs.UsersBirthdays, StatsBreadCrumbs.TodayVisitedUsers, StatsBreadCrumbs.SiteReferrers,
            UserProfilesBreadCrumbs.Users
        ]);
}
