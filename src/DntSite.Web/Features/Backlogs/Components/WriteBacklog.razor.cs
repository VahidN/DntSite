using AutoMapper;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Backlogs.Models;
using DntSite.Web.Features.Backlogs.RoutingConstants;
using DntSite.Web.Features.Backlogs.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;

namespace DntSite.Web.Features.Backlogs.Components;

[Authorize]
public partial class WriteBacklog
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    public IList<string>? AutoCompleteDataList { get; set; }

    [SupplyParameterFromForm(FormName = nameof(WriteBacklog))]
    public BacklogModel WriteBacklogModel { get; set; } = new();

    [InjectComponentScoped] internal ITagsService TagsService { set; get; } = null!;

    [Parameter] public string? EditId { set; get; }

    [Parameter] public string? DeleteId { set; get; }

    [InjectComponentScoped] internal IBacklogsService BacklogsService { set; get; } = null!;

    [Inject] internal IMapper Mapper { set; get; } = null!;

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (!ApplicationState.CanCurrentUserCreateANewBacklog())
        {
            Alert.ShowAlert(AlertType.Danger, title: "عدم دسترسی",
                message: "برای ایجاد یک پیشنهاد جدید نیاز است حداقل دو لینک اشتراکی ارسالی در سایت داشته باشید.");

            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        AutoCompleteDataList = await TagsService.GetTagNamesArrayAsync(count: 2000);
        AddBreadCrumbs();

        if (!ApplicationState.HttpContext.IsGetRequest())
        {
            return;
        }

        await PerformPossibleDeleteAsync();

        await FillPossibleEditFormAsync();
    }

    private async Task PerformPossibleDeleteAsync()
    {
        if (string.IsNullOrWhiteSpace(DeleteId))
        {
            return;
        }

        var backlog = await GetUserBacklogAsync(DeleteId.ToInt());
        await BacklogsService.MarkAsDeletedAsync(backlog);
        await BacklogsService.NotifyDeleteChangesAsync(backlog, new BacklogModel());

        ApplicationState.NavigateTo(BacklogsRoutingConstants.Backlogs);
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..BacklogsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task FillPossibleEditFormAsync()
    {
        if (string.IsNullOrWhiteSpace(EditId))
        {
            return;
        }

        var item = await GetUserBacklogAsync(EditId.ToInt());

        if (item is null)
        {
            return;
        }

        WriteBacklogModel = Mapper.Map<Backlog, BacklogModel>(item);
    }

    private async Task<Backlog?> GetUserBacklogAsync(int id)
    {
        var item = await BacklogsService.GetFullBacklogAsync(id);

        if (item is null || !ApplicationState.CanCurrentUserEditThisItem(item.UserId, item.Audit.CreatedAt))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return null;
        }

        return item;
    }

    private async Task PerformAsync()
    {
        var user = ApplicationState.CurrentUser?.User;

        Backlog? backlog;

        if (!string.IsNullOrWhiteSpace(EditId))
        {
            backlog = await GetUserBacklogAsync(EditId.ToInt());
            await BacklogsService.UpdateBacklogAsync(backlog, WriteBacklogModel);
        }
        else
        {
            backlog = await BacklogsService.AddBacklogAsync(WriteBacklogModel, user);
        }

        await BacklogsService.NotifyAddOrUpdateChangesAsync(backlog, WriteBacklogModel);

        ApplicationState.NavigateTo(string.Create(CultureInfo.InvariantCulture,
            $"{BacklogsRoutingConstants.BacklogsDetailsBase}/{backlog?.Id}"));
    }
}
