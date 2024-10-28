using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.RoutingConstants;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.AppConfigs.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class SystemLogs
{
    private const int ItemsPerPage = 10;

    private const string MainTitle = "گزارش رخ‌دادهای سیستم";

    private PagedResultModel<AppLogItemModel>? _pagedResults;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public string? CurrentLogLevel { set; get; }

    [InjectComponentScoped] internal IAppLogItemsService AppLogItemsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [SupplyParameterFromForm] internal int RowId { set; get; }

    private string BasePath => string.IsNullOrWhiteSpace(CurrentLogLevel)
        ? AppConfigsRoutingConstants.SystemLogs
        : $"{AppConfigsRoutingConstants.SystemLogs}/{CurrentLogLevel}";

    [SupplyParameterFromForm] internal int? CutOffDays { set; get; }

    private LogLevel? CurrentLogLevelValue => CurrentLogLevel.ToLogLevel();

    protected override async Task OnInitializedAsync()
    {
        await ShowResultsAsync();
        AddBreadCrumbs();
    }

    private async Task ShowResultsAsync()
        => _pagedResults = await AppLogItemsService.GetPagedAppLogItemsAsync(new DntQueryBuilderModel
        {
            IsAscending = false,
            Page = CurrentPage ?? 1,
            PageSize = ItemsPerPage
        }, CurrentLogLevelValue);

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([AppConfigsBreadCrumbs.SystemLogsBreadCrumb]);

    private async Task LogItemDeleteAsync()
    {
        await AppLogItemsService.DeleteAsync(RowId);
        await ShowResultsAsync();
    }

    private async Task LogDeleteAllAsync()
    {
        await AppLogItemsService.DeleteAllAsync(CurrentLogLevelValue);
        await ShowResultsAsync();
    }

    private async Task LogDeleteOlderThanAsync()
    {
        if (!CutOffDays.HasValue)
        {
            return;
        }

        var cutoffUtc = DateTime.UtcNow.AddDays(-CutOffDays.Value);

        await AppLogItemsService.DeleteOlderThanAsync(cutoffUtc, CurrentLogLevelValue);
        await ShowResultsAsync();
    }

    private static string GetDeleteLogRowFormName(int itemId)
        => string.Create(CultureInfo.InvariantCulture, $"DeleteLogRowForm{itemId}");
}
