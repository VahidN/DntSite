using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.RoadMaps.Entities;

public class LearningPath : BaseInteractiveEntity<LearningPath, LearningPathVisitor, LearningPathBookmark,
    LearningPathReaction, LearningPathTag, LearningPathComment, LearningPathCommentVisitor, LearningPathCommentBookmark,
    LearningPathCommentReaction, LearningPathUserFile, LearningPathUserFileVisitor>
{
    [StringLength(maximumLength: 450)] public required string Title { set; get; }

    [MaxLength] public required string Description { set; get; }
}
