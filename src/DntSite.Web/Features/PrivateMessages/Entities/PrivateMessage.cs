using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.PrivateMessages.Entities;

[IgnoreSoftDelete]
public class PrivateMessage : BaseInteractiveEntity<PrivateMessage, PrivateMessageVisitor, PrivateMessageBookmark,
    PrivateMessageReaction, PrivateMessageTag, PrivateMessageComment, PrivateMessageCommentVisitor,
    PrivateMessageCommentBookmark, PrivateMessageCommentReaction, PrivateMessageUserFile, PrivateMessageUserFileVisitor>

{
    public required string Title { set; get; }

    [MaxLength] public required string Body { set; get; }

    public bool EmailsSent { set; get; }

    public virtual User ToUser { set; get; } = null!;

    public int ToUserId { set; get; }

    public bool IsReadByReceiver { set; get; }
}
