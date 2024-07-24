using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Backlogs.Entities;

public class Backlog : BaseInteractiveEntity<Backlog, BacklogVisitor, BacklogBookmark, BacklogReaction, BacklogTag,
    BacklogComment, BacklogCommentVisitor, BacklogCommentBookmark, BacklogCommentReaction, BacklogUserFile,
    BacklogUserFileVisitor>
{
    public required string Title { set; get; }

    [MaxLength] public required string Description { set; get; }

    public virtual User? DoneByUser { set; get; }

    public int? DoneByUserId { set; get; }

    public DateTime? StartDate { set; get; }

    public DateTime? DoneDate { set; get; }

    public int? DaysEstimate { set; get; }

    public virtual BlogPost? ConvertedBlogPost { set; get; }

    public int? ConvertedBlogPostId { set; get; }
}
