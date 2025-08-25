using AutoMapper;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.RoadMaps.Models;
using DntSite.Web.Features.RoadMaps.RoutingConstants;
using DntSite.Web.Features.RoadMaps.Services.Contracts;

namespace DntSite.Web.Features.RoadMaps.Components;

[Authorize]
public partial class WriteLearningPaths
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    public IList<string>? AutoCompleteDataList { get; set; }

    [SupplyParameterFromForm(FormName = nameof(WriteLearningPaths))]
    public LearningPathModel WriteLearningPathModel { get; set; } = new();

    [InjectComponentScoped] internal ITagsService TagsService { set; get; } = null!;

    [Parameter] public string? EditId { set; get; }

    [Parameter] public string? DeleteId { set; get; }

    [InjectComponentScoped] internal ILearningPathService LearningPathService { set; get; } = null!;

    [Inject] internal IMapper Mapper { set; get; } = null!;

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();

        AutoCompleteDataList = await TagsService.GetTagNamesArrayAsync(count: 2000);

        if (!ApplicationState.HttpContext.IsGetRequest())
        {
            return;
        }

        if (!ApplicationState.CanCurrentUserCreateALearningPath())
        {
            Alert.ShowAlert(AlertType.Danger, title: "خطا!",
                message: "امکان تهیه نقشه‌های راه صرفا برای نویسندگان سایت وجود دارد.");

            ApplicationState.NavigateToUnauthorizedPage();

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

        var learningPathItem = await GetUserLearningPathItemAsync(DeleteId.ToInt());
        await LearningPathService.MarkAsDeletedAsync(learningPathItem);
        await LearningPathService.NotifyDeleteChangesAsync(learningPathItem);

        ApplicationState.NavigateTo(RoadMapsRoutingConstants.LearningPaths2);
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..RoadMapsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task FillPossibleEditFormAsync()
    {
        if (string.IsNullOrWhiteSpace(EditId))
        {
            return;
        }

        var item = await GetUserLearningPathItemAsync(EditId.ToInt());

        if (item is null)
        {
            return;
        }

        WriteLearningPathModel = Mapper.Map<LearningPath, LearningPathModel>(item);
    }

    private async Task<LearningPath?> GetUserLearningPathItemAsync(int id)
    {
        var learningPathItem = await LearningPathService.GetLearningPathAsync(id);

        if (learningPathItem is null || !ApplicationState.CanCurrentUserEditThisItem(learningPathItem.UserId))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return null;
        }

        return learningPathItem;
    }

    private async Task PerformAsync()
    {
        var user = ApplicationState.CurrentUser?.User;

        LearningPath? learningPathItem;

        if (!string.IsNullOrWhiteSpace(EditId))
        {
            learningPathItem = await GetUserLearningPathItemAsync(EditId.ToInt());
            await LearningPathService.UpdateLearningPathAsync(learningPathItem, WriteLearningPathModel);
        }
        else
        {
            learningPathItem = await LearningPathService.AddLearningPathAsync(WriteLearningPathModel, user);
        }

        await LearningPathService.NotifyAddOrUpdateChangesAsync(learningPathItem);

        ApplicationState.NavigateTo(string.Create(CultureInfo.InvariantCulture,
            $"{RoadMapsRoutingConstants.LearningPathsDetailsBase}/{learningPathItem?.Id}"));
    }
}
