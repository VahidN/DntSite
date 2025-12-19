using AutoMapper;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Models;
using DntSite.Web.Features.StackExchangeQuestions.RoutingConstants;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

namespace DntSite.Web.Features.StackExchangeQuestions.Components;

[Authorize]
public partial class WriteQuestion
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    public IList<string>? AutoCompleteDataList { get; set; }

    [SupplyParameterFromForm(FormName = nameof(WriteQuestion))]
    public QuestionModel? WriteQuestionModel { get; set; }

    [InjectComponentScoped] internal ITagsService TagsService { set; get; } = null!;

    [Parameter] public string? EditId { set; get; }

    [Parameter] public string? DeleteId { set; get; }

    [InjectComponentScoped] internal IQuestionsService QuestionsService { set; get; } = null!;

    [Inject] internal IMapper Mapper { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        WriteQuestionModel ??= new QuestionModel();
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

        var question = await GetStackExchangeQuestionAsync(DeleteId.ToInt());
        await QuestionsService.MarkAsDeletedAsync(question);
        await QuestionsService.NotifyDeleteChangesAsync(question, ApplicationState.CurrentUser?.User);

        ApplicationState.NavigateTo(QuestionsRoutingConstants.Questions);
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..QuestionsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task FillPossibleEditFormAsync()
    {
        if (string.IsNullOrWhiteSpace(EditId))
        {
            return;
        }

        var item = await GetStackExchangeQuestionAsync(EditId.ToInt());

        if (item is null)
        {
            return;
        }

        WriteQuestionModel = Mapper.Map<StackExchangeQuestion, QuestionModel>(item);
    }

    private async Task<StackExchangeQuestion?> GetStackExchangeQuestionAsync(int id)
    {
        var question = await QuestionsService.GetStackExchangeQuestionAsync(id);

        if (question is null || !ApplicationState.CanCurrentUserEditThisItem(question.UserId, question.Audit.CreatedAt))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return null;
        }

        return question;
    }

    private async Task PerformAsync()
    {
        var user = ApplicationState.CurrentUser?.User;

        StackExchangeQuestion? question;

        if (!string.IsNullOrWhiteSpace(EditId))
        {
            question = await GetStackExchangeQuestionAsync(EditId.ToInt());
            await QuestionsService.UpdateQuestionsItemAsync(question, WriteQuestionModel, user);
        }
        else
        {
            question = await QuestionsService.AddStackExchangeQuestionAsync(WriteQuestionModel, user);
        }

        await QuestionsService.NotifyAddOrUpdateChangesAsync(question, WriteQuestionModel, user);

        ApplicationState.NavigateTo(string.Create(CultureInfo.InvariantCulture,
            $"{QuestionsRoutingConstants.QuestionsDetailsBase}/{question?.Id}"));
    }
}
