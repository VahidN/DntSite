using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class ChangeUserPassword
{
    private string? _userFriendlyName;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [SupplyParameterFromForm] public ChangeUserPasswordModel Model { get; set; } = new();

    [Parameter] public string? UserId { set; get; }

    private string PageTitle => $"تغییر کلمه عبور «{_userFriendlyName ?? "کاربر"}»";

    [InjectComponentScoped] internal IUsersInfoService UsersService { set; get; } = null!;

    [Inject] public IProtectionProviderService ProtectionProvider { set; get; } = null!;

    [InjectComponentScoped] internal IUserProfilesManagerService UserProfilesManagerService { set; get; } = null!;

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    [InjectComponentScoped] internal IUsersManagerEmailsService UsersManagerEmailsService { set; get; } = null!;

    private string EncryptedUserId => ProtectionProvider.Encrypt(UserId.ToInt().ToString(CultureInfo.InvariantCulture));

    protected override async Task OnInitializedAsync()
    {
        var user = await UsersService.FindUserAsync(UserId.ToInt());

        if (user is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _userFriendlyName = user.FriendlyName;

        AddBreadCrumbs();
    }

    private async Task PerformAsync()
    {
        if (Model is null)
        {
            Alert.ShowAlert(AlertType.Danger, title: "خطا!",
                message: "اطلاعات وارد شده معتبر نیستند. لطفا مجددا سعی کنید.");

            return;
        }

        var user = await UsersService.FindUserAsync(UserId.ToInt());

        if (user is null)
        {
            Alert.ShowAlert(AlertType.Danger, title: "خطا!", message: "لطفا مجددا وارد سیستم شوید.");

            return;
        }

        var (message, stat) = await UserProfilesManagerService.ChangeUserPasswordAsync(user.Id, Model.NewPassword);

        switch (stat)
        {
            case OperationStat.Failed:
                Alert.ShowAlert(AlertType.Danger, title: "خطا!", message);

                return;
            case OperationStat.Succeeded:
                await UsersManagerEmailsService.UserProfileSendEmailAsync(new UserProfileEmailModel
                {
                    Username = user.UserName,
                    FriendlyName = user.FriendlyName,
                    OriginalPassword = Model.NewPassword
                }, user);

                Alert.ShowAlert(AlertType.Success, title: "با تشکر!", message);

                ResetForm();

                break;
        }
    }

    private void ResetForm() => Model = new ChangeUserPasswordModel();

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            UserProfilesBreadCrumbs.Users, new BreadCrumb
            {
                Title = "تغییر کلمه‌ی عبور کاربر",
                Url = Invariant($"{UserProfilesRoutingConstants.ChangeUserPasswordBase}/{EncryptedUserId}"),
                GlyphIcon = DntBootstrapIcons.BiPerson
            }
        ]);
}
