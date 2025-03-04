using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Projects.Entities;

public class ProjectFaqComment : BaseCommentsEntity<ProjectFaqComment, ProjectFaq, ProjectFaqCommentVisitor,
    ProjectFaqCommentBookmark, ProjectFaqCommentReaction>;
