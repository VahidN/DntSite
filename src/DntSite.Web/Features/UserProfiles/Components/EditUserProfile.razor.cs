using AutoMapper;
using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Components;

[Authorize]
public partial class EditUserProfile
{
    private string? _userFriendlyName;

    [Parameter] public string? EditUserId { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    [InjectComponentScoped] internal IUserProfilesManagerService UserProfilesManagerService { set; get; } = null!;

    [Inject] internal IMapper Mapper { set; get; } = null!;

    [InjectComponentScoped] internal ICurrentUserService CurrentUserService { set; get; } = null!;

    [SupplyParameterFromForm] public UserProfileModel? Model { get; set; }

    private string PageTitle => $"تنظیمات کاربری من «{_userFriendlyName}»";

    protected override async Task OnInitializedAsync()
    {
        Model ??= new UserProfileModel();
        var currentUser = await CurrentUserService.GetCurrentImpersonatedUserAsync(EditUserId.ToInt());

        if (currentUser is null)
        {
            ApplicationState.NavigateTo(UserProfilesRoutingConstants.Login);

            return;
        }

        if (ApplicationState.HttpContext.IsGetRequest())
        {
            await UserProfilesManagerService.UpdateUserImageFromGravatarAsync(currentUser);
            Model = Mapper.Map<User, UserProfileModel>(currentUser);
        }

        _userFriendlyName = currentUser.FriendlyName;

        AddBreadCrumbs(currentUser);
    }

    private void AddBreadCrumbs(User currentUser)
        => ApplicationState.BreadCrumbs.AddRange([
            new BreadCrumb
            {
                Title = "مشخصات من",
                Url = $"{UserProfilesRoutingConstants.Users}/{Uri.EscapeDataString(currentUser.FriendlyName)}",
                GlyphIcon = DntBootstrapIcons.BiPerson
            }
        ]);

    private async Task PerformAsync()
    {
        var operationResult = await UserProfilesManagerService.EditUserProfileAsync(Model, EditUserId.ToInt());

        switch (operationResult.Stat)
        {
            case OperationStat.Failed:
                Alert.ShowAlert(AlertType.Danger, title: "خطا!", operationResult.Message);

                break;
            case OperationStat.Succeeded:
                ApplicationState.NavigationManager.NavigateTo(
                    $"{UserProfilesRoutingConstants.Users}/{Uri.EscapeDataString(Model?.FriendlyName ?? _userFriendlyName ?? "")}");

                break;
        }
    }
}
