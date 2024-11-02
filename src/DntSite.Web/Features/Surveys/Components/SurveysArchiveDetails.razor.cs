using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.Models;
using DntSite.Web.Features.Surveys.ModelsMappings;
using DntSite.Web.Features.Surveys.RoutingConstants;
using DntSite.Web.Features.Surveys.Services.Contracts;

namespace DntSite.Web.Features.Surveys.Components;

public partial class SurveysArchiveDetails
{
    private string? _documentTypeIdHash;
    private List<SurveyComment>? _surveyComments;

    private BlogVoteModel? _surveys;

    [Parameter] public int? SurveyId { set; get; }

    private Survey? CurrentPost => _surveys?.CurrentItem;

    private DateTime? ModifiedAt => CurrentPost?.AuditActions.Count > 0
        ? CurrentPost?.AuditActions[^1].CreatedAt
        : CurrentPost?.Audit.CreatedAt;

    private string? CurrentPostImageUrl
        => HtmlHelperService.ExtractImagesLinks(CurrentPost?.Description ?? "").FirstOrDefault();

    [InjectComponentScoped] internal IVotesService SurveysService { set; get; } = null!;

    [InjectComponentScoped] internal IVoteCommentsService SurveyCommentsService { set; get; } = null!;

    [Inject] internal IHtmlHelperService HtmlHelperService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public string? Slug { set; get; }

    private bool CanUserDeleteThisPost => ApplicationState.IsCurrentUserAdmin;

    private bool CanUserEditThisPost
        => ApplicationState.CanCurrentUserEditThisItem(CurrentPost?.UserId, CurrentPost?.Audit.CreatedAt);

    private List<string> GetTags() => CurrentPost?.Tags.Select(x => x.Name).ToList() ?? [];

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();

        if (!SurveyId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _surveys = await SurveysService.GetBlogVoteLastAndNextPostAsync(SurveyId.Value);

        if (_surveys.CurrentItem is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        await GetCommentsAsync(SurveyId.Value);

        SetSimilarPostsId();
    }

    private void SetSimilarPostsId()
        => _documentTypeIdHash = _surveys?.CurrentItem?.MapToWhatsNewItemModel(siteRootUri: "").DocumentTypeIdHash;

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..SurveysBreadCrumbs.DefaultBreadCrumbs]);

    private async Task GetCommentsAsync(int id)
        => _surveyComments = await SurveyCommentsService.GetRootCommentsOfVoteAsync(id);

    private async Task HandleCommentActionAsync(CommentActionModel model)
    {
        switch (model.CommentAction)
        {
            case CommentAction.Delete:
                await SurveyCommentsService.DeleteCommentAsync(model.FormCommentId);

                break;
            case CommentAction.SubmitEditedComment:
                await SurveyCommentsService.EditReplyAsync(model.FormCommentId, model.Comment ?? "");

                break;
            case CommentAction.SubmitNewComment:
                await SurveyCommentsService.AddReplyAsync(model.FormCommentId, model.FormPostId, model.Comment ?? "",
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

    private async Task HandleVoteActionAsync((int FormId, IList<int> SurveyItemIds) arg)
    {
        await SurveysService.ApplyVoteAsync(arg.FormId, arg.SurveyItemIds, ApplicationState.CurrentUser?.User);

        ApplicationState.NavigateTo(string.Create(CultureInfo.InvariantCulture,
            $"{SurveysRoutingConstants.SurveysArchiveDetailsBase}/{arg.FormId}#results"));
    }
}
