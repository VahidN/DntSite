using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Posts.Components;

public partial class ShowBlogPostCommentItem<TReactionEntity, TForeignKeyEntity>
    where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>, new()
    where TForeignKeyEntity : BaseAuditedInteractiveEntity
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public bool ShowPostTitle { set; get; }

    [Parameter] public string? PostTitle { set; get; }

    [Parameter] public bool ShowPleaseVoteMessage { set; get; } = true;

    [Parameter] public bool ShowReactions { set; get; } = true;

    [Parameter] public bool ShowManageBlogPostCommentItem { set; get; } = true;

    [Parameter] [EditorRequired] public int CommentId { set; get; }

    [Parameter] [EditorRequired] public int PostId { set; get; }

    [Parameter] public RenderFragment? AdditionalBodyContent { set; get; }

    [Parameter] public RenderFragment? AdditionalInfoContent { set; get; }

    [Parameter] [EditorRequired] public required ICollection<TReactionEntity> Reactions { get; set; }

    [Parameter] [EditorRequired] public required string CreatedByUserAgent { set; get; }

    [Parameter] [EditorRequired] public required string Body { set; get; }

    [Parameter] [EditorRequired] public required User RecordUser { set; get; }

    [Parameter] [EditorRequired] public DateTime CreatedAt { set; get; }

    [Parameter] public required string UploadFileApiPath { get; set; }

    [Parameter] public required string UploadImageFileApiPath { get; set; }

    [Parameter] public Func<CommentActionModel, Task>? HandleCommentAction { set; get; }

    [Parameter] public string? PostAbsoluteUrl { get; set; }

    [Parameter] public RenderFragment? AdditionalButtonsContent { set; get; }

    private string PostUrl => string.IsNullOrWhiteSpace(PostAbsoluteUrl)
        ? ApplicationState.CurrentAbsoluteUri.ToString()
        : PostAbsoluteUrl;

    private string PostUrlWithComment => string.Create(CultureInfo.InvariantCulture, $"{PostUrl}#comment-{CommentId}");

    private string CommentHtmlId => string.Create(CultureInfo.InvariantCulture, $"comment-{CommentId}");
}
