using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.PrivateMessages.Models;
using DntSite.Web.Features.PrivateMessages.RoutingConstants;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;

namespace DntSite.Web.Features.PrivateMessages.Components;

public partial class PublicContactUs
{
    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    [InjectComponentScoped] internal IPrivateMessagesEmailsService EmailsService { set; get; } = null!;

    [Inject] internal IAppFoldersService AppFoldersService { set; get; } = null!;

    [SupplyParameterFromForm] public PublicContactUsModel? Model { get; set; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private async Task PerformAsync()
    {
        if (Model is null)
        {
            Alert.ShowAlert(AlertType.Danger, title: "خطا!",
                message: "اطلاعات وارد شده معتبر نیستند. لطفا مجددا سعی کنید.");

            return;
        }

        await EmailsService.SendPublicContactUsAsync(Model);
        Alert.ShowAlert(AlertType.Success, title: "با تشکر!", message: " پیام شما ارسال شد.");

        ResetForm();
    }

    private void ResetForm() => Model = new PublicContactUsModel();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Model ??= new PublicContactUsModel();

        AddBreadCrumbs();
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([..PrivateMessagesBreadCrumbs.DefaultBreadCrumbs]);
}
