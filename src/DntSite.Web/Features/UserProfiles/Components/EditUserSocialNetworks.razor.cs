using AutoMapper;
using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Components;

[Authorize]
public partial class EditUserSocialNetworks
{
    private string? _userFriendlyName;

    [Parameter] public int? EditUserId { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    [InjectComponentScoped] internal IUsersInfoService UsersService { set; get; } = null!;

    [InjectComponentScoped] internal IUserProfilesManagerService UserProfilesManagerService { set; get; } = null!;

    [InjectComponentScoped] internal ICurrentUserService CurrentUserService { set; get; } = null!;

    [Inject] internal IMapper Mapper { set; get; } = null!;

    [SupplyParameterFromForm] public UserSocialNetworkModel Model { get; set; } = new();

    private string PageTitle => $"تنظیمات کاربری من «{_userFriendlyName}» در شبکه‌های اجتماعی";

    protected override async Task OnInitializedAsync()
    {
        var currentUser = await CurrentUserService.GetCurrentImpersonatedUserAsync(EditUserId);

        if (ApplicationState.HttpContext.IsGetRequest())
        {
            var userSocialNetwork = await UsersService.FindUserSocialNetworkAsync(currentUser?.Id);

            if (userSocialNetwork is null)
            {
                ApplicationState.HttpContext.SsrRedirectTo(UserProfilesRoutingConstants.Login);

                return;
            }

            Model = Mapper.Map<UserSocialNetwork, UserSocialNetworkModel>(userSocialNetwork);
        }

        _userFriendlyName = currentUser?.FriendlyName;
        AddBreadCrumbs(_userFriendlyName);
    }

    private void AddBreadCrumbs(string? friendlyName)
        => ApplicationState.BreadCrumbs.AddRange([
            new BreadCrumb
            {
                Title = "مشخصات من",
                Url = string.IsNullOrWhiteSpace(friendlyName)
                    ? UserProfilesRoutingConstants.Users
                    : $"{UserProfilesRoutingConstants.Users}/{Uri.EscapeDataString(friendlyName)}",
                GlyphIcon = DntBootstrapIcons.BiPerson
            }
        ]);

    private async Task PerformAsync()
    {
        var operationResult = await UserProfilesManagerService.EditUserSocialNetworksAsync(EditUserId, Model);

        switch (operationResult.Stat)
        {
            case OperationStat.Failed:
                Alert.ShowAlert(AlertType.Danger, title: "خطا!", operationResult.Message);

                break;
            case OperationStat.Succeeded:
                ApplicationState.NavigationManager.NavigateTo(
                    $"{UserProfilesRoutingConstants.Users}/{Uri.EscapeDataString(_userFriendlyName ?? "")}");

                break;
        }
    }
}
