using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.UserProfiles.Entities;

public class UserProfileComment : BaseCommentsEntity<UserProfileComment, User, UserProfileCommentVisitor,
    UserProfileCommentBookmark, UserProfileCommentReaction>;
