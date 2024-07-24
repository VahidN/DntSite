using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Components;

public partial class JobSeekers
{
    private List<User>? _users;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [InjectComponentScoped] internal IUsersInfoService UsersService { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();
        _users = await UsersService.GetJobSeekersListAsNoTrackingAsync();
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([UserProfilesBreadCrumbs.Users, UserProfilesBreadCrumbs.JobSeekers]);
}
