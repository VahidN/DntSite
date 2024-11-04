using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Layout;

public partial class HeaderUserProfileMenu
{
    private CurrentUserModel? _currentUser;
    private int _unreadPrivateMessagesCount;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [InjectComponentScoped] internal IPrivateMessagesService PrivateMessagesService { set; get; } = null!;

    private string GetUserUrl(string baseHref)
        => $"{baseHref}/{Uri.EscapeDataString(_currentUser?.User?.FriendlyName ?? "")}";

    protected override async Task OnInitializedAsync()
    {
        _currentUser = ApplicationState.CurrentUser;

        _unreadPrivateMessagesCount =
            await PrivateMessagesService.GetUserUnReadPrivateMessagesCountAsync(_currentUser?.UserId);
    }
}
