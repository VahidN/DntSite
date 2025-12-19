using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Components;

public partial class Register
{
    private bool _canUsersRegister;

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    [InjectComponentScoped] internal IUserProfilesManagerService UsersService { set; get; } = null!;

    [InjectComponentScoped] internal ICurrentUserService CurrentUserService { set; get; } = null!;

    [Inject] internal IAppFoldersService AppFoldersService { set; get; } = null!;

    [SupplyParameterFromForm] public RegisterModel? Model { get; set; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        Model ??= new RegisterModel();
        ApplicationState.DoNotLogPageReferrer = true;

        await CurrentUserService.ClearExistingAuthenticationCookiesAsync(clearAdminCookies: false);
        _canUsersRegister = await CurrentUserService.CanCurrentUserRegisterAsync();

        AddBreadCrumbs();
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([UserProfilesBreadCrumbs.Login]);

    private async Task PerformAsync()
    {
        var currentUserId = CurrentUserService.GetCurrentUserId();
        var operationResult = await UsersService.RegisterUserAsync(Model, _canUsersRegister, currentUserId);

        switch (operationResult.Stat)
        {
            case OperationStat.Failed:
                Alert.ShowAlert(AlertType.Danger, title: "خطا!", operationResult.Message);

                break;
            case OperationStat.Succeeded:
                Alert.ShowAlert(AlertType.Success, title: "با تشکر!", operationResult.Message);
                ResetForm();

                break;
        }
    }

    private void ResetForm() => Model = new RegisterModel();
}
