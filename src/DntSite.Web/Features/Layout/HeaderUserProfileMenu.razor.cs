using DntSite.Web.Features.PrivateMessages.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Layout;

public partial class HeaderUserProfileMenu
{
    private int _unreadPrivateMessagesCount;

    [InjectComponentScoped] internal IPrivateMessagesService PrivateMessagesService { set; get; } = null!;

    [Parameter] [EditorRequired] public CurrentUserModel? CurrentUser { set; get; }

    private string GetUserUrl(string baseHref)
        => $"{baseHref}/{Uri.EscapeDataString(CurrentUser?.User?.FriendlyName ?? "")}";

    protected override async Task OnParametersSetAsync()
        => _unreadPrivateMessagesCount =
            await PrivateMessagesService.GetUserUnReadPrivateMessagesCountAsync(CurrentUser?.UserId);
}
