using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Backlogs.Entities;

public class BacklogComment : BaseCommentsEntity<BacklogComment, Backlog, BacklogCommentVisitor, BacklogCommentBookmark,
    BacklogCommentReaction>;
