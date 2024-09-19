using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Posts.Components;

public partial class ManageBlogPostCommentItem
{
    private bool _showSubmitEditedComment;
    private bool _showSubmitNewComment;

    private string FormName => string.Create(CultureInfo.InvariantCulture, $"CommentAction_{CommentId}");

    private bool CanUserEditThisReply => ApplicationState.CanCurrentUserEditThisItem(RecordUser?.Id, CreatedAt);

    [Parameter] public bool ShowPleaseVoteMessage { set; get; } = true;

    [Parameter] [EditorRequired] public User? RecordUser { set; get; }

    [Parameter] [EditorRequired] public bool IsReplyToPost { set; get; }

    [Parameter] [EditorRequired] public DateTime? CreatedAt { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] [EditorRequired] public int? CommentId { set; get; }

    [Parameter] [EditorRequired] public int PostId { set; get; }

    [SupplyParameterFromForm] public CommentAction CommentActionValue { set; get; }

    [SupplyParameterFromForm] public int? FormCommentId { set; get; }

    [SupplyParameterFromForm] public int FormPostId { set; get; }

    [SupplyParameterFromForm] public string? Comment { set; get; }

    [Parameter] [EditorRequired] public string? Body { set; get; }

    [Parameter] [EditorRequired] public Func<CommentActionModel, Task>? HandleCommentAction { set; get; }

    [Parameter] [EditorRequired] public required string UploadFileApiPath { get; set; }

    [Parameter] [EditorRequired] public required string UploadImageFileApiPath { get; set; }

    private async Task OnValidSubmitAsync()
    {
        _showSubmitNewComment = false;
        _showSubmitEditedComment = false;

        if (CommentActionValue == CommentAction.Cancel)
        {
            return;
        }

        if (HandleCommentAction is null)
        {
            throw new InvalidOperationException($"{nameof(HandleCommentAction)} is null.");
        }

        if (!ApplicationState.CanCurrentUserPostAComment(CommentActionValue, RecordUser?.Id, CreatedAt))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        switch (CommentActionValue)
        {
            case CommentAction.ReplyToPost:
            case CommentAction.ReplyToComment:
                Comment = "";
                _showSubmitNewComment = true;

                break;
            case CommentAction.Edit:
                Comment = Body;
                _showSubmitEditedComment = true;

                break;
            case CommentAction.SubmitEditedComment:
            case CommentAction.SubmitNewComment:
            case CommentAction.Delete:
                await HandleCommentAction.Invoke(new CommentActionModel
                {
                    Comment = Comment,
                    CommentAction = CommentActionValue,
                    FormCommentId = FormCommentId,
                    FormPostId = FormPostId
                });

                break;
        }
    }
}
