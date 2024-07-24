using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Components;

[Authorize]
public partial class ChangePassword
{
    private DateTime? _lastUserPasswordChangeDate;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    [InjectComponentScoped] internal IUsersManagerEmailsService UsersManagerEmailsService { set; get; } = null!;

    [InjectComponentScoped] internal IUserProfilesManagerService UsersService { set; get; } = null!;

    [InjectComponentScoped] internal IUsedPasswordsService UsedPasswordsService { set; get; } = null!;

    [SupplyParameterFromForm] public ChangePasswordModel Model { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var currentUser = ApplicationState.CurrentUser;

        if (currentUser is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _lastUserPasswordChangeDate = await UsedPasswordsService.GetLastUserPasswordChangeDateAsync(currentUser.UserId);

        AddBreadCrumbs(currentUser);
    }

    private void AddBreadCrumbs(CurrentUserModel currentUser)
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
        if (Model is null)
        {
            Alert.ShowAlert(AlertType.Danger, title: "خطا!",
                message: "اطلاعات وارد شده معتبر نیستند. لطفا مجددا سعی کنید.");

            return;
        }

        var currentUser = ApplicationState.CurrentUser?.User;

        if (currentUser is null)
        {
            Alert.ShowAlert(AlertType.Danger, title: "خطا!", message: "لطفا مجددا وارد سیستم شوید.");

            return;
        }

        var (message, stat) = await UsersService.ChangeUserPasswordAsync(currentUser.Id, Model.NewPassword);

        switch (stat)
        {
            case OperationStat.Failed:
                Alert.ShowAlert(AlertType.Danger, title: "خطا!", message);

                return;
            case OperationStat.Succeeded:
                await UsersManagerEmailsService.UserProfileSendEmailAsync(new UserProfileEmailModel
                {
                    Username = currentUser.UserName,
                    FriendlyName = currentUser.FriendlyName,
                    OriginalPassword = Model.NewPassword
                }, currentUser);

                Alert.ShowAlert(AlertType.Success, title: "با تشکر!", message);
                ResetForm();

                break;
        }
    }

    private void ResetForm() => Model = new ChangePasswordModel();
}
