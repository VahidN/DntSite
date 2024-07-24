using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Projects.Entities;

public class ProjectIssueComment : BaseCommentsEntity<ProjectIssueComment, ProjectIssue, ProjectIssueCommentVisitor,
    ProjectIssueCommentBookmark, ProjectIssueCommentReaction>
{
    public bool EmailsSent { set; get; }
}
