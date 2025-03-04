using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.RoadMaps.Entities;

public class LearningPathComment : BaseCommentsEntity<LearningPathComment, LearningPath, LearningPathCommentVisitor,
    LearningPathCommentBookmark, LearningPathCommentReaction>;
