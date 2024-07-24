using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Projects.Entities;

public class ProjectComment : BaseCommentsEntity<ProjectComment, Project, ProjectCommentVisitor, ProjectCommentBookmark,
    ProjectCommentReaction>
{
}
