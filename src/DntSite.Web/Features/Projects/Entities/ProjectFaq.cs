using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Projects.Entities;

public class ProjectFaq : BaseInteractiveEntity<ProjectFaq, ProjectFaqVisitor, ProjectFaqBookmark, ProjectFaqReaction,
    ProjectFaqTag, ProjectFaqComment, ProjectFaqCommentVisitor, ProjectFaqCommentBookmark, ProjectFaqCommentReaction,
    ProjectFaqUserFile, ProjectFaqUserFileVisitor>
{
    [StringLength(maximumLength: 450)] public required string Title { set; get; }

    [MaxLength] public required string Description { set; get; }

    public virtual Project Project { set; get; } = null!;

    public int ProjectId { set; get; }
}
