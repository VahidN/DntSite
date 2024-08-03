using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class UsersManager
{
    private const int ItemsPerPage = 10;

    private string? _basePath;
    private PagedResultModel<User>? _users;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [InjectComponentScoped] internal IUsersInfoService UsersService { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public string? Filter { set; get; }

    protected override async Task OnInitializedAsync()
    {
        await ShowUsersListAsync(Filter);
        AddBreadCrumbs();
    }

    private async Task ShowUsersListAsync(string? gridifyFilter)
    {
        CurrentPage ??= 1;

        _basePath =
            $"{UserProfilesRoutingConstants.UsersManagerFilterBase}/{Uri.EscapeDataString(gridifyFilter ?? "*")}";

        _users = await UsersService.GetUsersListAsNoTrackingAsync(new DntQueryBuilderModel
        {
            GridifyFilter = gridifyFilter.NormalizeGridifyFilter(),
            IsAscending = true,
            Page = CurrentPage.Value,
            PageSize = ItemsPerPage,
            SortBy = nameof(User.Id)
        });
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([UserProfilesBreadCrumbs.Users, UserProfilesBreadCrumbs.UsersManager]);

    private Task DoSearchAsync(string gridifyFilter)
    {
        ApplicationState.NavigateTo(
            $"{UserProfilesRoutingConstants.UsersManagerFilterBase}/{Uri.EscapeDataString(gridifyFilter ?? "*")}/page/1");

        return Task.CompletedTask;
    }
}
