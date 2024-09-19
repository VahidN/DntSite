using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Models;
using DntSite.Web.Features.StackExchangeQuestions.RoutingConstants;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

namespace DntSite.Web.Features.StackExchangeQuestions.Components;

public partial class MarkQuestionCommentAsAnswer
{
    private string FormName
        => string.Create(CultureInfo.InvariantCulture, $"MarkQuestionCommentAsAnswer_{QuestionComment?.Id}");

    private bool CanCurrentUserMarkAsAnswer => ApplicationState.CurrentUser?.UserId == QuestionComment?.Parent.UserId ||
                                               ApplicationState.CurrentUser?.IsAdmin == true;

    private bool IsThisCommentAnswer => QuestionComment?.IsAnswer == true;

    [SupplyParameterFromForm] public MarkAsAnswerAction AnswerReaction { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public bool IsQuestionAnswered { set; get; }

    [Parameter] public StackExchangeQuestionComment? QuestionComment { set; get; }

    [InjectComponentScoped] internal IQuestionsCommentsService QuestionsCommentsService { set; get; } = null!;

    private async Task OnValidSubmitAsync()
    {
        if (!CanCurrentUserMarkAsAnswer)
        {
            return;
        }

        switch (AnswerReaction)
        {
            case MarkAsAnswerAction.ThumbsUp:
                await QuestionsCommentsService.MarkQuestionCommentAsAnswerAsync(QuestionComment, isAnswer: true);

                break;
            case MarkAsAnswerAction.Cancel:
                await QuestionsCommentsService.MarkQuestionCommentAsAnswerAsync(QuestionComment, isAnswer: false);

                break;
        }

        ApplicationState.NavigateTo(string.Create(CultureInfo.InvariantCulture,
            $"{QuestionsRoutingConstants.QuestionsDetailsBase}/{QuestionComment?.ParentId}"));
    }
}
