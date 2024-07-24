using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.PrivateMessages.Models;
using DntSite.Web.Features.PrivateMessages.RoutingConstants;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.PrivateMessages.Components;

[Authorize]
public partial class SendPrivateMessage
{
    private string? _friendlyName;

    [Parameter] public int? ToUserId { set; get; }

    [Parameter] public int? EditId { set; get; }

    [Parameter] public int? DeleteId { set; get; }

    [InjectComponentScoped] internal IUsersInfoService UsersService { set; get; } = null!;

    [InjectComponentScoped] internal IPrivateMessagesService PrivateMessagesService { set; get; } = null!;

    [SupplyParameterFromForm] public ContactUsModel Model { set; get; } = new();

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (!ApplicationState.HttpContext.IsGetRequest())
        {
            return;
        }

        if (DeleteId.HasValue)
        {
            await TryDeletePrivateMessageAsync(DeleteId.Value);

            return;
        }

        if (EditId.HasValue)
        {
            await TryInitEditDataAsync(EditId.Value);

            return;
        }

        await InitFriendlyNameAsync();
    }

    private async Task TryInitEditDataAsync(int id)
    {
        var firstPrivateMessage =
            await PrivateMessagesService.GetFirstAllowedPrivateMessageAsync(id, ApplicationState.CurrentUser?.UserId);

        if (firstPrivateMessage is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _friendlyName = firstPrivateMessage.ToUser.FriendlyName;
        Model.Title = firstPrivateMessage.Title;
        Model.DescriptionText = firstPrivateMessage.Body;
    }

    private async Task TryDeletePrivateMessageAsync(int id)
    {
        var firstPrivateMessage =
            await PrivateMessagesService.GetFirstAllowedPrivateMessageAsync(id, ApplicationState.CurrentUser?.UserId);

        if (firstPrivateMessage is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        await PrivateMessagesService.RemovePrivateMessageAsync(id);
        ApplicationState.NavigateTo($"{PrivateMessagesRoutingConstants.MyPrivateMessages}#main");
    }

    private async Task InitFriendlyNameAsync()
    {
        var toUser = await UsersService.FindUserAsync(ToUserId);

        if (toUser is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _friendlyName = toUser.FriendlyName;
    }

    private async Task PerformAsync()
    {
        if (EditId.HasValue)
        {
            await EditPrivateMessageAsync(EditId.Value);
        }
        else
        {
            await AddPrivateMessageAsync();
        }
    }

    private async Task EditPrivateMessageAsync(int id)
    {
        await PrivateMessagesService.EditFirstPrivateMessageAsync(id, ApplicationState.CurrentUser?.UserId, Model);
        ApplicationState.NavigateTo(Invariant($"/{PrivateMessagesRoutingConstants.MyPrivateMessageBase}/{id}#main"));
    }

    private async Task AddPrivateMessageAsync()
    {
        var operationResult =
            await PrivateMessagesService.AddPrivateMessageAsync(ApplicationState.CurrentUser?.User, ToUserId, Model);

        switch (operationResult.Stat)
        {
            case OperationStat.Failed:
                Alert.ShowAlert(AlertType.Danger, title: "خطا!", operationResult.Message);

                break;
            case OperationStat.Succeeded:
                ApplicationState.NavigateTo($"{PrivateMessagesRoutingConstants.MyPrivateMessages}#main");

                break;
        }
    }
}
