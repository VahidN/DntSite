using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Posts.Components;

public partial class ShowBlogPostItem<TReactionEntity, TForeignKeyEntity>
    where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>, new()
    where TForeignKeyEntity : BaseAuditedInteractiveEntity
{
    [Parameter] public bool ShowReactions { set; get; } = true;

    [Parameter] public bool ShowSocialLinks { set; get; } = true;

    [Parameter] public bool ShowNumberOfViews { set; get; } = true;

    [Parameter] public bool ShowNumberOfComments { set; get; } = true;

    [Parameter] [EditorRequired] public required ICollection<TReactionEntity> Reactions { get; set; }

    [Parameter] [EditorRequired] public bool ShowBriefDescription { set; get; }

    [Parameter] [EditorRequired] public bool ShowTags { set; get; }

    [Parameter] [EditorRequired] public bool ShowCommentsButton { set; get; }

    [Parameter] [EditorRequired] public int Id { set; get; }

    [Parameter] [EditorRequired] public required string CommentsUrlTemplate { set; get; }

    private string CommentsUrl => string.Format(CultureInfo.InvariantCulture, CommentsUrlTemplate, Id);

    [Parameter] [EditorRequired] public required string PostUrlTemplate { set; get; }

    private string PostUrl => !EncryptPostUrl
        ? string.Format(CultureInfo.InvariantCulture, PostUrlTemplate, Id)
        : string.Format(CultureInfo.InvariantCulture, PostUrlTemplate,
            ProtectionProvider.Encrypt(Id.ToString(CultureInfo.InvariantCulture)));

    [Parameter] public bool EncryptPostUrl { set; get; }

    [Parameter] [EditorRequired] public required string PostTagUrlTemplate { set; get; }

    [Parameter] [EditorRequired] public required string Title { set; get; }

    [Parameter] [EditorRequired] public required string Body { set; get; }

    [Parameter] [EditorRequired] public string? BriefDescription { set; get; }

    [Parameter] [EditorRequired] public int? ReadingTimeMinutes { set; get; }

    [Parameter] [EditorRequired] public required string CreatedByUserAgent { set; get; }

    [Parameter] [EditorRequired] public required User RecordUser { set; get; }

    [Parameter] [EditorRequired] public required DateTime CreatedAt { set; get; }

    [Parameter] [EditorRequired] public int NumberOfViewsFromFeed { set; get; }

    [Parameter] [EditorRequired] public int NumberOfViews { set; get; }

    [Parameter] [EditorRequired] public int NumberOfComments { set; get; }

    [Parameter] [EditorRequired] public IReadOnlyList<string>? Tags { set; get; }

    [Parameter] [EditorRequired] public required string SiteName { set; get; }

    [Parameter] [EditorRequired] public bool CanUserEditThisPost { set; get; }

    [Parameter] [EditorRequired] public bool CanUserDeleteThisPost { set; get; }

    [Parameter] [EditorRequired] public required string DeletePostUrlTemplate { set; get; }

    [Parameter] [EditorRequired] public bool EncryptEditDeleteIDs { set; get; }

    private string DeletePostUrl => !EncryptEditDeleteIDs
        ? string.Format(CultureInfo.InvariantCulture, DeletePostUrlTemplate, Id)
        : string.Format(CultureInfo.InvariantCulture, DeletePostUrlTemplate,
            ProtectionProvider.Encrypt(Id.ToString(CultureInfo.InvariantCulture)));

    [Parameter] [EditorRequired] public required string EditPostUrlTemplate { set; get; }

    [Parameter] public RenderFragment? AdditionalBodyContent { set; get; }

    [Parameter] public RenderFragment? AdditionalButtonsContentForOwner { set; get; }

    [Parameter] public RenderFragment? AdditionalButtonsContentForAll { set; get; }

    [Parameter] public RenderFragment? AdditionalHeaderContent { set; get; }

    [Parameter] public RenderFragment? BeforeHeaderContent { set; get; }

    [Parameter] public RenderFragment? AdditionalInfoContent { set; get; }

    [Parameter] public RenderFragment? AfterFooterContent { set; get; }

    [Inject] public IProtectionProviderService ProtectionProvider { set; get; } = null!;

    private string EditPostUrl => !EncryptEditDeleteIDs
        ? string.Format(CultureInfo.InvariantCulture, EditPostUrlTemplate, Id)
        : string.Format(CultureInfo.InvariantCulture, EditPostUrlTemplate,
            ProtectionProvider.Encrypt(Id.ToString(CultureInfo.InvariantCulture)));

    private string TextToShow => !ShowBriefDescription ? Body : BriefDescription ?? "";

    private string GetTagUrl(string tagName)
        => string.Format(CultureInfo.InvariantCulture, PostTagUrlTemplate, Uri.EscapeDataString(tagName));
}
