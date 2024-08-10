using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Stats.RoutingConstants;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.RoutingConstants;

namespace DntSite.Web.Features.Stats.Components;

public partial class TodayVisitedUsers
{
    private const int PostItemsPerPage = 10;
    private int _totalUsersCount;

    private IList<(User User, string BadgeValue)>? _users;

    [Parameter] public int? CurrentPage { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [InjectComponentScoped] internal ISiteStatService SiteStatService { set; get; } = null!;

    [Inject] internal IOnlineVisitorsService OnlineVisitorsService { set; get; } = null!;

    protected override async Task OnInitializedAsync() => await ShowUsersListAsync();

    private async Task ShowUsersListAsync()
    {
        CurrentPage ??= 1;

        var results = await SiteStatService.GetPagedTodayVisitedUsersListAsync(CurrentPage.Value - 1, PostItemsPerPage);

        _users = results.Data.Select(user => (user, user.LastVisitDateTime.ToFriendlyPersianDateTextify())).ToList();

        _totalUsersCount = results.TotalItems;

        AddUsersListBreadCrumbs();
    }

    private void AddUsersListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            UserProfilesBreadCrumbs.UsersBirthdays, StatsBreadCrumbs.TodayVisitedUsers, UserProfilesBreadCrumbs.Users
        ]);
}
