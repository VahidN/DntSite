using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.PrivateMessages.Entities;

public class PrivateMessageComment : BaseCommentsEntity<PrivateMessageComment, PrivateMessage,
    PrivateMessageCommentVisitor, PrivateMessageCommentBookmark, PrivateMessageCommentReaction>
{
}
