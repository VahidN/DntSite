using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Utils.WebToolkit;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Models;
using DntSite.Web.Features.StackExchangeQuestions.ModelsMappings;
using DntSite.Web.Features.StackExchangeQuestions.RoutingConstants;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

namespace DntSite.Web.Features.StackExchangeQuestions.Components;

public partial class QuestionsArchiveDetails
{
    private QuestionDetailsModel? _details;

    private string? _documentTypeIdHash;

    private List<StackExchangeQuestionComment>? _questionComments;

    [Parameter] public int? QuestionId { set; get; }

    private StackExchangeQuestion? CurrentPost => _details?.CurrentItem;

    private DateTime? ModifiedAt => CurrentPost?.AuditActions.Count > 0
        ? CurrentPost?.AuditActions[^1].CreatedAt
        : CurrentPost?.Audit.CreatedAt;

    private string? CurrentPostImageUrl
        => HtmlHelperService.ExtractImagesLinks(CurrentPost?.Description ?? "").FirstOrDefault();

    [InjectComponentScoped] internal IQuestionsService QuestionsService { set; get; } = null!;

    [InjectComponentScoped] internal IQuestionsCommentsService QuestionsCommentsService { set; get; } = null!;

    [Inject] internal IHtmlHelperService HtmlHelperService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public string? Slug { set; get; }

    private bool CanUserDeleteThisPost => ApplicationState.CurrentUser?.IsAdmin == true;

    private bool CanUserEditThisPost
        => ApplicationState.CanCurrentUserEditThisItem(CurrentPost?.UserId, CurrentPost?.Audit.CreatedAt);

    private List<string> GetTags() => CurrentPost?.Tags.Select(x => x.Name).ToList() ?? [];

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();

        if (!QuestionId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _details = await QuestionsService.GetQuestionDetailsAsync(QuestionId.Value);

        if (_details.CurrentItem is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        await GetCommentsAsync(QuestionId.Value);
        await UpdateStatAsync();
        SetSimilarPostsId();
    }

    private void SetSimilarPostsId()
        => _documentTypeIdHash = _details?.CurrentItem?.MapToWhatsNewItemModel(siteRootUri: "").DocumentTypeIdHash;

    private async Task UpdateStatAsync()
    {
        if (ApplicationState.HttpContext.IsPostRequest())
        {
            return;
        }

        await QuestionsService.UpdateStatAsync(_details?.CurrentItem, ApplicationState.NavigationManager.IsFromFeed());
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..QuestionsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task GetCommentsAsync(int id)
        => _questionComments = await QuestionsCommentsService.GetRootCommentsOfQuestionAsync(id);

    private async Task HandleCommentActionAsync(CommentActionModel model)
    {
        switch (model.CommentAction)
        {
            case CommentAction.Delete:
                await QuestionsCommentsService.DeleteCommentAsync(model.FormCommentId);

                break;
            case CommentAction.SubmitEditedComment:
                await QuestionsCommentsService.EditReplyAsync(model.FormCommentId, model.Comment ?? "");

                break;
            case CommentAction.SubmitNewComment:
                await QuestionsCommentsService.AddReplyAsync(model.FormCommentId, model.FormPostId, model.Comment ?? "",
                    ApplicationState.CurrentUser?.UserId ?? 0,
                    ApplicationState.CurrentUser?.User?.IsRestricted ?? true);

                break;
            case CommentAction.ReplyToComment:
            case CommentAction.Edit:
            case CommentAction.Cancel:
            default:
                break;
        }

        await GetCommentsAsync(model.FormPostId);
        StateHasChanged();
    }
}
