using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Projects.Entities;

public class ProjectReleaseComment : BaseCommentsEntity<ProjectReleaseComment, ProjectRelease,
    ProjectReleaseCommentVisitor, ProjectReleaseCommentBookmark, ProjectReleaseCommentReaction>;
