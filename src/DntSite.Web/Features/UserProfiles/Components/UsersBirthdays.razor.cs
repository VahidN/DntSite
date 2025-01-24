using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Models;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.RoutingConstants;

namespace DntSite.Web.Features.UserProfiles.Components;

public partial class UsersBirthdays
{
    private AgeStatModel? _ageStat;
    private List<User>? _users;

    [InjectComponentScoped] internal IStatService StatService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();
        _users = await StatService.GetTodayBirthdayListAsync(SharedConstants.AYearAgo);
        _ageStat = await StatService.GetAverageAgeAsync();
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([..UserProfilesBreadCrumbs.DefaultBreadCrumbs]);
}
