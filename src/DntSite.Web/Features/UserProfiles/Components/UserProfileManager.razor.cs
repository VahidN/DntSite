using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Components;

public partial class UserProfileManager
{
    private string FormName => Invariant($"{nameof(UserProfileManager)}{User?.Id}");

    private bool IsUserAdmin => User is not null && User.Roles.Any(x => string.Equals(x.Name, CustomRoles.Admin,
        StringComparison.Ordinal));

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [InjectComponentScoped] internal IUserProfilesManagerService UsersService { set; get; } = null!;

    [InjectComponentScoped] internal IUserRolesService RolesService { set; get; } = null!;

    [SupplyParameterFromForm] public AdminAction AdminActionValue { set; get; }

    [Parameter] [EditorRequired] public User? User { set; get; }

    [Inject] public IProtectionProviderService ProtectionProvider { set; get; } = null!;

    private string EncryptedUserId
        => User is null ? "" : ProtectionProvider.Encrypt(User.Id.ToString(CultureInfo.InvariantCulture));

    private async Task PerformAsync()
    {
        if (ApplicationState.CurrentUser?.IsAdmin == false)
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        if (User is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        switch (AdminActionValue)
        {
            case AdminAction.MakeRestricted:
                await UsersService.UserIsRestrictedAsync(User.Id);

                break;
            case AdminAction.MakeUnRestricted:
                await UsersService.UserIsNotRestrictedAsync(User.Id);

                break;
            case AdminAction.MakeInActive:
                await UsersService.DisableUserAsync(User.Id);

                break;
            case AdminAction.MakeActive:
                await UsersService.ActivateUserAsync(User.Id);

                break;
            case AdminAction.SendEmailValidationMessage:
                await UsersService.SendActivateYourAccountEmailAsync(User.Id);

                break;
            case AdminAction.MakeAdmin:
                await RolesService.AddOrUpdateUserRolesAsync(User.Id, [CustomRoles.Admin]);

                break;
            case AdminAction.RemoveAdmin:
                await RolesService.AddOrUpdateUserRolesAsync(User.Id, []);

                break;
            default:
                throw new NotSupportedException($"{AdminActionValue} is not supported");
        }

        ApplicationState.NavigateTo($"{UserProfilesRoutingConstants.Users}/{Uri.EscapeDataString(User.FriendlyName)}");
    }
}
