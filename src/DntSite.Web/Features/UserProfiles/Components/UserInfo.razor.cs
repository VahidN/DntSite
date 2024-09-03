using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Stats.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Components;

public partial class UserInfo
{
    private const int ItemsPerPage = 10;
    private string? _basePath;
    private User? _user;
    private PagedResultModel<User>? _users;

    private string PageTitle => _user?.FriendlyName ?? "کاربران";

    [Parameter] public string? Name { get; set; }

    [Parameter] public string? Filter { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [InjectComponentScoped] internal IUsersInfoService UsersService { set; get; } = null!;

    [InjectComponentScoped] internal IUserProfilesManagerService UserProfilesManagerService { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [InjectComponentScoped] internal ISearchItemsService SearchItemsService { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            await ShowUsersListAsync(Filter);
            AddUsersBreadCrumbs();
        }
        else
        {
            await ShowUserProfileAsync(Name);
            AddUserBreadCrumbs(Name);
        }
    }

    private async Task ShowUsersListAsync(string? gridifyFilter)
    {
        CurrentPage ??= 1;

        _basePath = $"{UserProfilesRoutingConstants.UsersFilterBase}/{Uri.EscapeDataString(gridifyFilter ?? "*")}";

        _users = await UsersService.GetUsersListAsNoTrackingAsync(new DntQueryBuilderModel
        {
            GridifyFilter = gridifyFilter.NormalizeGridifyFilter(),
            IsAscending = false,
            Page = CurrentPage.Value,
            PageSize = ItemsPerPage,
            SortBy = nameof(User.LastVisitDateTime)
        });
    }

    private async Task DoSearchAsync(string gridifyFilter)
    {
        await SearchItemsService.AddSearchItemAsync(gridifyFilter);

        ApplicationState.NavigateTo(
            $"{UserProfilesRoutingConstants.UsersFilterBase}/{Uri.EscapeDataString(gridifyFilter ?? "*")}/page/1");
    }

    private async Task ShowUserProfileAsync(string name)
    {
        var user = await UsersService.FindUserByFriendlyNameIncludeUserSocialNetworkAsync(name);

        if (user is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        await UserProfilesManagerService.UpdateUserImageFromGravatarAsync(user);
        _user = user;
    }

    private void AddUsersBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([..StatsBreadCrumbs.OnlineUsersStatsBreadCrumbs]);

    private void AddUserBreadCrumbs(string name)
    {
        AddUsersBreadCrumbs();

        ApplicationState.BreadCrumbs.AddRange([
            new BreadCrumb
            {
                Title = name,
                Url = $"{UserProfilesRoutingConstants.Users}/{Uri.EscapeDataString(name)}",
                GlyphIcon = DntBootstrapIcons.BiPerson
            }
        ]);
    }
}
