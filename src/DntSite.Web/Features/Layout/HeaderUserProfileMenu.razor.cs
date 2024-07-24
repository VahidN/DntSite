using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;

namespace DntSite.Web.Features.Layout;

public partial class HeaderUserProfileMenu
{
    private int _unreadPrivateMessagesCount;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [InjectComponentScoped] internal IPrivateMessagesService PrivateMessagesService { set; get; } = null!;

    private string GetUserUrl(string baseHref)
        => $"{baseHref}/{Uri.EscapeDataString(ApplicationState.CurrentUser?.User?.FriendlyName ?? "")}";

    protected override async Task OnInitializedAsync()
        => _unreadPrivateMessagesCount =
            await PrivateMessagesService.GetUserUnReadPrivateMessagesCountAsync(ApplicationState.CurrentUser?.UserId);
}
