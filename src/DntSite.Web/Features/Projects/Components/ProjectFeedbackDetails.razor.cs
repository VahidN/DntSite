using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.Projects.ModelsMappings;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Components;

public partial class ProjectFeedbackDetails
{
    private string? _documentTypeIdHash;
    private List<ProjectIssueComment>? _issueComments;
    private ProjectIssueDetailsModel? _issueTopic;

    [Parameter] public int? ProjectId { set; get; }

    [Parameter] public int? FeedbackId { set; get; }

    private ProjectIssue? CurrentPost => _issueTopic?.CurrentItem;

    private DateTime? ModifiedAt => CurrentPost?.AuditActions.Count > 0
        ? CurrentPost?.AuditActions[^1].CreatedAt
        : CurrentPost?.Audit.CreatedAt;

    private string? CurrentPostImageUrl
        => HtmlHelperService.ExtractImagesLinks(CurrentPost?.Description ?? "").FirstOrDefault();

    [InjectComponentScoped] internal IProjectIssuesService ProjectIssuesService { set; get; } = null!;

    [InjectComponentScoped] internal IProjectIssueCommentsService ProjectIssueCommentsService { set; get; } = null!;

    [Inject] internal IHtmlHelperService HtmlHelperService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public string? Slug { set; get; }

    private bool CanUserDeleteThisPost => ApplicationState.IsCurrentUserAdmin;

    private bool CanUserEditThisPost
        => ApplicationState.CanCurrentUserEditThisItem(CurrentPost?.UserId, CurrentPost?.Audit.CreatedAt);

    private string CommentsUrlTemplate => string.Create(CultureInfo.InvariantCulture,
        $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{ProjectId}/{FeedbackId}#comments");

    private string PostUrlTemplate => string.Create(CultureInfo.InvariantCulture,
        $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{ProjectId}/{FeedbackId}");

    private string LastPostUrl => string.Create(CultureInfo.InvariantCulture,
        $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{ProjectId}/{_issueTopic!.PreviousItem?.Id}");

    private string NextPostUrl => string.Create(CultureInfo.InvariantCulture,
        $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{ProjectId}/{_issueTopic!.NextItem?.Id}");

    private string EditPostUrlTemplate => string.Create(CultureInfo.InvariantCulture,
        $"{ProjectsRoutingConstants.WriteProjectFeedbackEditBase}/{ProjectId}/{EncryptedFeedbackId}");

    private string DeletePostUrlTemplate => string.Create(CultureInfo.InvariantCulture,
        $"{ProjectsRoutingConstants.WriteProjectFeedbackDeleteBase}/{ProjectId}/{EncryptedFeedbackId}");

    private string EncryptedFeedbackId
        => Uri.EscapeDataString(ProtectionProvider.Encrypt(FeedbackId?.ToString(CultureInfo.InvariantCulture)) ?? "");

    [Inject] public IProtectionProviderService ProtectionProvider { set; get; } = null!;

    private List<string> GetTags() => CurrentPost?.Tags.Select(x => x.Name).ToList() ?? [];

    protected override async Task OnInitializedAsync()
    {
        if (!ProjectId.HasValue || !FeedbackId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _issueTopic =
            await ProjectIssuesService.GetProjectIssueLastAndNextPostIncludeAuthorTagsAsync(ProjectId.Value,
                FeedbackId.Value);

        if (_issueTopic?.CurrentItem is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        await ProjectIssuesService.UpdateNumberOfViewsAsync(FeedbackId.Value,
            ApplicationState.NavigationManager.IsFromFeed());

        await GetCommentsAsync(_issueTopic.CurrentItem.Id);

        AddBreadCrumbs(_issueTopic?.CurrentItem?.Project.Title ?? "");
        SetSimilarPostsId();
    }

    private void SetSimilarPostsId()
        => _documentTypeIdHash = _issueTopic?.CurrentItem
            ?.MapToProjectsIssuesWhatsNewItemModel(siteRootUri: "", showBriefDescription: false)
            .DocumentTypeIdHash;

    private void AddBreadCrumbs(string name)
        => ApplicationState.BreadCrumbs.AddRange([..ProjectsBreadCrumbs.DefaultProjectBreadCrumbs(name, ProjectId)]);

    private async Task GetCommentsAsync(int id)
        => _issueComments = await ProjectIssueCommentsService.GetRootCommentsOfIssuesAsync(id);

    private async Task HandleCommentActionAsync(CommentActionModel model)
    {
        switch (model.CommentAction)
        {
            case CommentAction.Delete:
                await ProjectIssueCommentsService.DeleteCommentAsync(model.FormCommentId);

                break;
            case CommentAction.SubmitEditedComment:
                await ProjectIssueCommentsService.EditReplyAsync(model.FormCommentId, model.Comment ?? "");

                break;
            case CommentAction.SubmitNewComment:
                await ProjectIssueCommentsService.AddReplyAsync(model.FormCommentId, model.FormPostId,
                    model.Comment ?? "", ApplicationState.CurrentUser?.UserId ?? 0);

                break;
            case CommentAction.ReplyToComment:
            case CommentAction.Edit:
            case CommentAction.Cancel:
            default:
                return;
        }

        await GetCommentsAsync(model.FormPostId);
        StateHasChanged();
    }
}
