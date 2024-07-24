using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class SendActivationEmails
{
    private const int FromDays = -31;
    private int _numberOfNotActivatedUsers;
    private int _numberOfNotActivatedUsersOfLastMonth;

    [SupplyParameterFromForm] public SendActivationEmailsAction SendActivationEmailsAction { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    [InjectComponentScoped] internal IUsersInfoService UsersService { set; get; } = null!;

    [InjectComponentScoped] internal IUsersManagerEmailsService EmailsService { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await InitStatAsync();

        AddBreadCrumbs();
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            UserProfilesBreadCrumbs.Users, UserProfilesBreadCrumbs.UsersManager,
            UserProfilesBreadCrumbs.SendActivationEmails
        ]);

    private async Task InitStatAsync()
    {
        _numberOfNotActivatedUsers = await UsersService.NumberOfNotActivatedUsersAsync(from: null);

        _numberOfNotActivatedUsersOfLastMonth =
            await UsersService.NumberOfNotActivatedUsersAsync(DateTime.UtcNow.AddDays(FromDays));
    }

    private async Task PerformAsync()
    {
        switch (SendActivationEmailsAction)
        {
            case SendActivationEmailsAction.ActivateAll:
                await EmailsService.ResetNotActivatedUsersAndSendEmailAsync(from: null);

                break;
            case SendActivationEmailsAction.ActivateOnlyLastMonth:
                await EmailsService.ResetNotActivatedUsersAndSendEmailAsync(DateTime.UtcNow.AddDays(FromDays));

                break;
        }

        Alert.ShowAlert(AlertType.Success, title: "با تشکر!", message: " ایمیل‌های فعالسازی ارسال شدند.");
    }
}
