using AutoMapper;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.Models;
using DntSite.Web.Features.Surveys.RoutingConstants;
using DntSite.Web.Features.Surveys.Services.Contracts;

namespace DntSite.Web.Features.Surveys.Components;

[Authorize]
public partial class WriteSurvey
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    public IList<string>? AutoCompleteDataList { get; set; }

    [SupplyParameterFromForm(FormName = nameof(WriteSurvey))]
    public VoteModel WriteSurveyModel { get; set; } = new();

    [InjectComponentScoped] internal ITagsService TagsService { set; get; } = null!;

    [Parameter] public string? EditId { set; get; }

    [Parameter] public string? DeleteId { set; get; }

    [InjectComponentScoped] internal IVotesService SurveysService { set; get; } = null!;

    [Inject] internal IMapper Mapper { set; get; } = null!;

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (!ApplicationState.CanCurrentUserCreateANewSurvey())
        {
            Alert.ShowAlert(AlertType.Danger, title: "عدم دسترسی",
                message: "برای ایجاد یک نظرسنجی جدید نیاز است حداقل دو لینک اشتراکی ارسالی در سایت داشته باشید.");

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

        var surveyItem = await GetUserSurveyAsync(DeleteId.ToInt());
        await SurveysService.MarkAsDeletedAsync(surveyItem);
        await SurveysService.NotifyDeleteChangesAsync(surveyItem, ApplicationState.CurrentUser?.User);

        ApplicationState.NavigateTo(SurveysRoutingConstants.SurveysArchive);
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..SurveysBreadCrumbs.DefaultBreadCrumbs]);

    private async Task FillPossibleEditFormAsync()
    {
        if (string.IsNullOrWhiteSpace(EditId))
        {
            return;
        }

        var item = await GetUserSurveyAsync(EditId.ToInt());

        if (item is null)
        {
            return;
        }

        WriteSurveyModel = Mapper.Map<Survey, VoteModel>(item);
    }

    private async Task<Survey?> GetUserSurveyAsync(int id)
    {
        var surveyItem = await SurveysService.GetSurveyAsync(id);

        if (surveyItem is null ||
            !ApplicationState.CanCurrentUserEditThisItem(surveyItem.UserId, surveyItem.Audit.CreatedAt))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return null;
        }

        return surveyItem;
    }

    private async Task PerformAsync()
    {
        var user = ApplicationState.CurrentUser?.User;

        Survey? surveyItem;

        if (!string.IsNullOrWhiteSpace(EditId))
        {
            surveyItem = await GetUserSurveyAsync(EditId.ToInt());
            await SurveysService.UpdateSurveyAsync(surveyItem, WriteSurveyModel, user);
        }
        else
        {
            surveyItem = await SurveysService.AddNewsSurveyAsync(WriteSurveyModel, user);
        }

        await SurveysService.NotifyAddOrUpdateChangesAsync(surveyItem, WriteSurveyModel, user);

        ApplicationState.NavigateTo(Invariant($"{SurveysRoutingConstants.SurveysArchiveDetailsBase}/{surveyItem?.Id}"));
    }
}
