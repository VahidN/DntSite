using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Posts.Entities;

public class BlogPost : BaseInteractiveEntity<BlogPost, BlogPostVisitor, BlogPostBookmark, BlogPostReaction, BlogPostTag
    , BlogPostComment, BlogPostCommentVisitor, BlogPostCommentBookmark, BlogPostCommentReaction, BlogPostUserFile,
    BlogPostUserFileVisitor>
{
    public required string Title { set; get; }

    public string? BriefDescription { set; get; }

    [MaxLength] public required string Body { set; get; }

    public bool EmailsSent { set; get; }

    public int? NumberOfRequiredPoints { set; get; }

    public string? OldUrl { set; get; }

    public int ReadingTimeMinutes { set; get; }

    public bool PingbackSent { set; get; }

    public virtual ICollection<Backlog> Backlogs { set; get; } = [];
}
