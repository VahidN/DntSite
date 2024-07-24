using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.PrivateMessages.Entities;
using DntSite.Web.Features.PrivateMessages.RoutingConstants;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.PrivateMessages.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class PrivateMessagesViewer
{
    private const int ItemsPerPage = 10;
    private PagedResultModel<PrivateMessage>? _privateMessages;

    [InjectComponentScoped] internal IPrivateMessagesService PrivateMessagesService { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await ShowPrivateMessagesListAsync();
        AddBreadCrumbs();
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            PrivateMessagesBreadCrumbs.Users, PrivateMessagesBreadCrumbs.UsersManager,
            PrivateMessagesBreadCrumbs.PrivateMessagesViewer
        ]);

    private async Task ShowPrivateMessagesListAsync()
    {
        CurrentPage ??= 1;

        _privateMessages =
            await PrivateMessagesService.GetAllPrivateMessagesAsNoTrackingAsync(CurrentPage.Value - 1, ItemsPerPage);
    }
}
