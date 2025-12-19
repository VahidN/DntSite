using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class SendMassMail
{
    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    [InjectComponentScoped] internal IMassEmailsService MassEmailsService { set; get; } = null!;

    [InjectComponentScoped] internal IPrivateMessagesEmailsService EmailsFactoryService { set; get; } = null!;

    [InjectComponentScoped] internal IUsersInfoService UsersService { set; get; } = null!;

    [InjectComponentScoped] internal ICommonService CommonService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [SupplyParameterFromForm] public MassEmailModel? Model { get; set; }

    private async Task PerformAsync()
    {
        var body = await MassEmailsService.AddMassEmailAsync(Model, ApplicationState.CurrentUser?.UserId ?? 0);

        await SendEmailsToReadersAsync(body);
        await SendEmailsToWritersAsync(body);
        await SendEmailsToAdminsAsync(body);

        ResetForm();

        Alert.ShowAlert(AlertType.Success, title: "با تشکر!", message: " ایمیل‌ها ارسال شدند.");
    }

    private void ResetForm() => Model = new MassEmailModel();

    private async Task SendEmailsToAdminsAsync(string body)
    {
        if (Model is null)
        {
            return;
        }

        var adminsEmails = (await CommonService.GetAllActiveAdminsAsNoTrackingAsync()).Select(x => x.EMail).ToList();
        await EmailsFactoryService.SendMassEmailAsync(adminsEmails, $"اطلاعیه: {Model.NewsTitle.Trim()}", body);
    }

    private async Task SendEmailsToWritersAsync(string body)
    {
        if (Model?.Groups?.Contains(MassEmailGroup.Writer) != true)
        {
            return;
        }

        var writersEmails = (await UsersService.GetAllActiveUsersListWithMinPostsCountAsync(Model.MinPostsCount))
            .Select(x => x.EMail)
            .ToList();

        await EmailsFactoryService.SendMassEmailAsync(writersEmails, $"اطلاعیه: {Model.NewsTitle.Trim()}", body);
    }

    private async Task SendEmailsToReadersAsync(string body)
    {
        if (Model?.Groups?.Contains(MassEmailGroup.Reader) != true)
        {
            return;
        }

        var readersEmails = (await UsersService.GetAllActiveUsersListWithZeroPostCountAsync()).Select(x => x.EMail)
            .ToList();

        await EmailsFactoryService.SendMassEmailAsync(readersEmails, $"اطلاعیه: {Model.NewsTitle.Trim()}", body);
    }

    protected override void OnInitialized()
    {
        Model ??= new MassEmailModel();
        base.OnInitialized();
        AddBreadCrumbs();
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            UserProfilesBreadCrumbs.Users, UserProfilesBreadCrumbs.UsersManager, UserProfilesBreadCrumbs.SendMassMail
        ]);
}
