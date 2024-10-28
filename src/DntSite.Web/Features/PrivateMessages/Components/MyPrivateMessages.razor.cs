using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.PrivateMessages.Entities;
using DntSite.Web.Features.PrivateMessages.RoutingConstants;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;

namespace DntSite.Web.Features.PrivateMessages.Components;

[Authorize]
public partial class MyPrivateMessages
{
    private const int ItemsPerPage = 10;

    private const string MainTitle = "پیام‌های خصوصی من";

    private PagedResultModel<PrivateMessage>? _privateMessages;

    [InjectComponentScoped] internal IPrivateMessagesService PrivateMessagesService { set; get; } = null!;

    [Inject] public IProtectionProviderService ProtectionProvider { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    protected override async Task OnInitializedAsync()
    {
        await ShowResultsAsync();
        AddBreadCrumbs();
    }

    private async Task ShowResultsAsync()
    {
        var userId = ApplicationState.CurrentUser?.UserId;

        if (!userId.HasValue)
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        CurrentPage ??= 1;

        _privateMessages =
            await PrivateMessagesService.GetAllUserPrivateMessagesAsNoTrackingAsync(userId.Value, CurrentPage.Value - 1,
                ItemsPerPage);
    }

    private string GetPostUrl(PrivateMessage record)
        => PrivateMessagesRoutingConstants.MyPrivateMessageBase.CombineUrl(
            ProtectionProvider.Encrypt(record.Id.ToString(CultureInfo.InvariantCulture)), escapeRelativeUrl: true);

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            PrivateMessagesBreadCrumbs.Users, PrivateMessagesBreadCrumbs.MyPrivateMessages
        ]);

    private bool CanUserDeleteThisPost(PrivateMessage record) => ApplicationState.CanCurrentUserEditThisItem(record.Id);
}
